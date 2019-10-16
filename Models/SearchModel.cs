using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstantScheduler.Models
{
    public class SearchModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InStrings { get; set; }
        public string ExStrings { get; set; }
        public string InLocations { get; set; }
        public string ExLocations { get; set; }
        public bool InUsers { get; set; }
        public bool InHashtags { get; set; }
        public bool InPosts { get; set; }
        public bool InFollowingMe { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public UserModel User { get; set; }
        public List<TaskModel> Tasks { get; set; }

        public List<Location> GetInLocations()
        {
            return JsonConvert.DeserializeObject<List<Location>>(this.InLocations);
        }

        public List<Location> GetExLocations()
        {
            return JsonConvert.DeserializeObject<List<Location>>(this.ExLocations);
        }

        public void SetInLocations(List<Location> locations)
        {
            this.InLocations = JsonConvert.SerializeObject(locations);
        }

        public void SetExLocations(List<Location> locations)
        {
            this.ExLocations = JsonConvert.SerializeObject(locations);
        }

        public List<string> GetInStrings()
        {
            return JsonConvert.DeserializeObject<List<string>>(this.InStrings); 
        }

        public List<string> GetExStrings()
        {
            return JsonConvert.DeserializeObject<List<string>>(this.ExStrings);
        }

        public void SetInStrings(List<string> strings)
        {
            this.InStrings = JsonConvert.SerializeObject(strings);
        }

        public void SetExStrings(List<string> strings)
        {
            this.ExStrings = JsonConvert.SerializeObject(strings);
        }

    }
}