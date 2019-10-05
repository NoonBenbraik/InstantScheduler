﻿using InstantScheduler.Meta;
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
                return true; 
            }
        }
    }
}
