using InstantScheduler.DAL;
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
    /// Interaction logic for TaskItemView.xaml
    /// </summary>
    public partial class TaskItemView : UserControl
    {
        TaskModel Item; 
        public TaskItemView(TaskModel item)
        {
            InitializeComponent();
            this.Item = item; 

            lblName.Content = item.Name;
            progressBar.Value = (int)item.CompletedPercentage;

            if (!item.Active)
            {
                this.mainBorder.BorderBrush = Brushes.LightGray; 
            }

            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true; 
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //progressBar.Value = (int)Item.CompletedPercentage; 
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to stop this task premenantly?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                using (var context = new InstaContext())
                {
                    context.Tasks.FirstOrDefault(t => t.Id == this.Item.Id).Exectued = this.Item.Repeat + 1;
                    context.SaveChanges(); 
                }
            }

        }
    }
}
