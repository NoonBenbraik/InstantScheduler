using InstantScheduler.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantScheduler.DAL
{
    public class InstaContext : DbContext
    {
        public InstaContext() : base("name=DefaultConnection")
        {

           Database.SetInitializer<InstaContext>(new DropCreateDatabaseIfModelChanges<InstaContext>()); 

        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<ScheduleModel> Schedules { get; set; }
        public DbSet<SearchModel> Searches { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<UserModel>()
                .HasMany<ScheduleModel>(u => u.Schedules);

            modelBuilder.Entity<UserModel>()
                .HasMany<TaskModel>(u => u.Tasks);

            modelBuilder.Entity<UserModel>()
                .HasMany<SearchModel>(u => u.Searches);

            modelBuilder.Entity<ScheduleModel>()
                .HasRequired<UserModel>(s => s.User);

            modelBuilder.Entity<ScheduleModel>()
                .HasMany<TaskModel>(s => s.Tasks);

            modelBuilder.Entity<SearchModel>()
                .HasRequired<UserModel>(s => s.User);

            // Many to many? 
            modelBuilder.Entity<SearchModel>()
                .HasMany<TaskModel>(s => s.Tasks)
                .WithMany(t => t.Searches);

            modelBuilder.Entity<TaskModel>()
                .HasRequired<UserModel>(t => t.User);

            modelBuilder.Entity<TaskModel>()
                .HasRequired<ScheduleModel>(t => t.Schedule); 

            base.OnModelCreating(modelBuilder);

        }
    }
}
