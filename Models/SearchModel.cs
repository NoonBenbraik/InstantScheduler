using System;
using System.Collections.Generic;

namespace InstantScheduler.Models
{
    public class SearchModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string InStrings { get; set; }
        public string ExStrings { get; set; }
        public List<Location> InLocations { get; set; }
        public List<Location> ExLocations { get; set; }
        public bool InUsers { get; set; }
        public bool InHashtags { get; set; }
        public bool InPosts { get; set; }
        public bool InFollowingMe { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public UserModel User { get; set; }
        public List<TaskModel> Tasks { get; set; }
    }
}