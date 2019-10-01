using InstantScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantScheduler.DAL
{
    public static class UserProcessor
    {
        public static void CreateUser(UserModel user)
        {
            using (var context = new InstaContext())
            {
                context.Users.Add(user);
                context.SaveChangesAsync();
            }
        }


    }
}
