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
        public double CompletedPercentage
        {
            get
            {
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
    }
}
