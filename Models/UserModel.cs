using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantScheduler.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public long PK { get; set; }
        public DateTime LastLogin { get; set; }
        public List<TaskModel> Tasks { get; set; }
        public List<SearchModel> Searches { get; set; }
        public List<ScheduleModel> Schedules { get; set; }
    }
}
