using InstantScheduler.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantScheduler.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TaskType TaskType { get; set; }
        public ScheduleModel Schedule { get; set; }
        public List<SearchModel> Searches { get; set; }
        public int Repeat { get; set; }
        public int Exectued { get; set; }

    }
}
