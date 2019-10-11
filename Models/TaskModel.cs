using InstantScheduler.DAL;
using InstantScheduler.Meta;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantScheduler.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TaskType TaskType { get; set; }
        public UserModel User { get; set; }
        public ScheduleModel Schedule { get; set; }
        public List<SearchModel> Searches { get; set; }
        public string Values { get; set; }
        public int Repeat { get; set; }
        public int Exectued { get; set; }

        
        [NotMapped]
        public bool Active
        {
            get
            {
                Refresh();
                return this.Schedule.Active && this.CompletedPercentage <= 100;
            }
        }

        [NotMapped]
        public double CompletedPercentage
        {
            get
            {
                Refresh(); 
                return (((double)Exectued / Repeat) * 100); 
            }
        }

        public void SetValues(ValuesModel values)
        {
            this.Values = JsonConvert.SerializeObject(values); 
        }

        public ValuesModel GetValues()
        {
            return JsonConvert.DeserializeObject<ValuesModel>(this.Values); 
        }

        private void Refresh()
        {
            using (var context = new InstaContext())
            {
                this.Exectued = context.Tasks.First(t => t.Id == this.Id).Exectued;
            }
        }

        [NotMapped]
        public TaskModel Refreshed
        {
            get
            {
                using (var context = new InstaContext())
                {
                    return context.Tasks.Include("User").Include("Searches").Include("Schedule").FirstOrDefault(tsk => tsk.Id == this.Id); 
                }
            }
        }

    }
}
