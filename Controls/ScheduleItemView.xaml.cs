using InstantScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InstantScheduler.Controls
{
    /// <summary>
    /// Interaction logic for ScheduleItemView.xaml
    /// </summary>
    public partial class ScheduleItemView : UserControl
    {
        public ScheduleItemView(ScheduleModel item)
        {
            InitializeComponent();

            lblName.Content = item.Name;

            lblStartDate.Content = item.StartDate.ToString("YYYY-mm-dd"); 

            lblEndDate.Content = item.EndDate.ToString("YYYY-mm-dd");

            item.Days.ForEach(d =>
            {
                lblDays.Content += d.ToString() + ", ";
            });

            lblRunningTasks.Content = item.Tasks.Where(t => t.Exectued < t.Repeat).Count(); 

            lblCompletedTasks.Content = item.Tasks.Where(t => t.Exectued >= t.Repeat).Count();

            lblStatues.Content = item.Active ? "Active" : "Sleeping"; 
        }
    }
}
