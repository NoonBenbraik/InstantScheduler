using InstaSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantScheduler.Models
{
    public class ValuesModel
    {
        public string Text { get; set; } = "";
        public List<Image> Images { get; set; } = new List<Image>();
        public List<Video> Videos { get; set; } = new List<Video>(); 
    }
}
