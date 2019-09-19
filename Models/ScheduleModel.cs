using InstantScheduler.Meta;
using System;
using System.Collections.Generic;
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
        public List<DayOfWeek> Days { get; set; }
        public int TaskLimit { get; set; }
        public UserModel User { get; set; }
        public List<TaskModel> Tasks { get; set; }
    }
}
