using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantScheduler.Meta
{
    public class TimeModel
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public bool IsMorning { get; set; }


        [NotMapped]
        public static TimeModel Now
        {
            get
            {
                return new TimeModel
                {
                    Hour = DateTime.Now.Hour % 13,
                    Minute = DateTime.Now.Minute,
                    IsMorning = DateTime.Now.Hour <= 12
                }; 
            }
        }

        public static int Compare(TimeModel t1, TimeModel t2)
        {

            if (t1.Hour == 12 && t1.IsMorning)
                t1.Hour = 0;
            if (t2.Hour == 12 && t2.IsMorning)
                t2.Hour = 0; 

            t1.Hour = t1.IsMorning ? t1.Hour : t1.Hour + 12;
            t2.Hour = t2.IsMorning ? t2.Hour : t2.Hour + 12;

            int _t1 = (t1.Hour * 60) + t1.Minute;
            int _t2 = (t2.Hour * 60) + t2.Minute;

            if (_t1 > _t2)
                return 1;
            else if (_t1 == _t2)
                return 0;
            else
                return -1; 
        }
    }
}
