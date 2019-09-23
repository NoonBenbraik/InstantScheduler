using InstantScheduler.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantScheduler.DAL
{
    public class InstaContext : DbContext
    {
        public InstaContext() : base()
        {

        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<ScheduleModel> Schedules { get; set; }
        public DbSet<SearchModel> Searches { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }

    }
}
