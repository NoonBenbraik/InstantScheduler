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
    public class ScheduleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeModel StartTime { get; set; }
        public TimeModel EndTime { get; set; }
        public string Days { get; set; }
        public int TaskLimit { get; set; }

        public UserModel User { get; set; }
        public List<TaskModel> Tasks { get; set; }


        public List<DayOfWeek> GetDays()
        {
            return JsonConvert.DeserializeObject<List<DayOfWeek>>(Days);
        }

        public void SetDays(List<DayOfWeek> days)
        {
            this.Days = JsonConvert.SerializeObject(days);
        }

        [NotMapped]
        public bool Active
        {
            get
            {
                return GetDays().Contains(DateTime.Now.DayOfWeek)
                    && DateTime.Compare(DateTime.Now, this.StartDate) >= 0 && DateTime.Compare(DateTime.Now, this.EndDate) <= 0
                    && TimeModel.Compare(TimeModel.Now, this.StartTime) >= 0 && TimeModel.Compare(TimeModel.Now, this.EndTime) <= 0;
                //&& this.Tasks.Sum(t => t.Exectued) < this.TaskLimit; 
            }
        }

        [NotMapped]
        public int RemainingTasksCount
        {
            get
            {
                return this.TaskLimit - this.TaskRepeatCount + this.TaskExecutedCount; 
            }
        }

        [NotMapped]
        public int TaskExecutedCount
        {
            get { return this.Tasks.Sum(t => t.Exectued); }
        }

        [NotMapped]
        public int TaskRepeatCount
        {
            get { return this.Tasks.Sum(t => t.Repeat); }
        }
    }
}
