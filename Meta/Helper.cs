using InstantScheduler.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantScheduler.Meta
{
    public static class Helper
    {
        public static void LogTaskExecute(TaskModel t)
        {
            string directory = @"Meta\log\";
            string fileName = @"logger.txt";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(directory + fileName))
                File.Create(directory + fileName);

            File.AppendAllText(directory + fileName, JsonConvert.SerializeObject(t)); 
        }

        public static string PropsToString(object o)
        {
            string temp = "";
            o.GetType().GetProperties().ToList().ForEach(p =>
            {
                temp += p.Name + ": " + p.GetValue(o) + "\n";
            });

            return temp;
        }

    }
}
