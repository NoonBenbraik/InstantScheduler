using InstaSharper.API;
using InstaSharper.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantScheduler.DAL
{
    public static class InstagramAPI
    {
        public static IInstaApi API { get; set; }
        public static UserSessionData User { get; set; }
    }
}
