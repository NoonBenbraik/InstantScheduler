using InstantScheduler.DAL;
using InstantScheduler.Meta;
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
    /// Interaction logic for SchedulesView.xaml
    /// </summary>
    public partial class SchedulesView : UserControl
    {
        UserModel User; 

        public SchedulesView(UserModel User)
        {
            InitializeComponent();
            this.User = User;

            for(int i=1; i<60; i++)
            {
                comboStartTime_Min.Items.Add(i);
                comboEndTime_Min.Items.Add(i);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void checkEveryDay_Checked(object sender, RoutedEventArgs e)
        {
            checkSunday.IsEnabled = false;
            checkMonday.IsEnabled = false;
            checkTuseday.IsEnabled = false;
            checkWednesday.IsEnabled = false;
            checkThursday.IsEnabled = false;
            checkFriday.IsEnabled = false; 
            checkSaturday.IsEnabled = false; 
        }

        private void checkEveryDay_Unchecked(object sender, RoutedEventArgs e)
        {
            checkSunday.IsEnabled = true;
            checkMonday.IsEnabled = true;
            checkTuseday.IsEnabled = true;
            checkWednesday.IsEnabled = true;
            checkThursday.IsEnabled = true;
            checkFriday.IsEnabled = true;
            checkSaturday.IsEnabled = true;
        }

        private void txtDialyLimit_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = "";

            if (((TextBox)sender).Text.Any(c => !char.IsDigit(c)))
            {
                ((TextBox)sender).Text.ToList().ForEach(c =>
                {
                    if (char.IsDigit(c))
                        text += c;
                });

                ((TextBox)sender).Text = text;
            }
        }

        private void Reset()
        {

            txtName.Text = ""; 
            dateStartDate.SelectedDate = DateTime.Now;
            dateEndDate.SelectedDate = DateTime.Now.AddMonths(1);

            comboStartTime_Hour.SelectedIndex = 8;
            comboStartTime_Min.SelectedIndex = 0;
            comboStartTime_TOD.SelectedIndex = 0;

            comboEndTime_Hour.SelectedIndex = 4;
            comboEndTime_Min.SelectedIndex = 0;
            comboEndTime_TOD.SelectedIndex = 1;

            checkEveryDay.IsEnabled = true;
            checkSunday.IsEnabled = false;
            checkMonday.IsEnabled = false;
            checkTuseday.IsEnabled = false;
            checkWednesday.IsEnabled = false;
            checkThursday.IsEnabled = false;
            checkFriday.IsEnabled = false;
            checkSaturday.IsEnabled = false;

            checkEveryDay.IsChecked = true;
            checkSunday.IsChecked = false;
            checkMonday.IsChecked = false;
            checkTuseday.IsChecked = false;
            checkWednesday.IsChecked = false;
            checkThursday.IsChecked = false;
            checkFriday.IsChecked = false;
            checkSaturday.IsChecked = false;

            txtDialyLimit.Text = 100.ToString();


            using (var context = new InstaContext())
            {
                this.User = context.Users.Include("Schedules").Include("Searches").Include("Tasks").FirstOrDefault(u => u.Id == this.User.Id); 
            }

            pnlSchedules.Children.Clear();
            this.User.Schedules.ForEach(s => pnlSchedules.Children.Add(new ScheduleItemView(s))); 
        }

        private void LoadingState()
        {
            btnCreate.IsEnabled = false;
            btnReset.IsEnabled = false;
        }

        private void LoadedState()
        {
            btnCreate.IsEnabled = true;
            btnReset.IsEnabled = true;
        }



        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Reset?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if(result == MessageBoxResult.Yes)
                Reset();
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            LoadingState(); 

            var createdSchedule = new ScheduleModel
            {
                Name = txtName.Text,
                StartDate = dateStartDate.SelectedDate.GetValueOrDefault(DateTime.Now),
                EndDate = dateEndDate.SelectedDate.GetValueOrDefault(DateTime.Now.AddMonths(1)),
                StartTime = new TimeModel
                {
                    Hour = int.Parse(comboStartTime_Hour.Text), 
                    Minute = int.Parse(comboStartTime_Min.Text), 
                    IsMorning = comboStartTime_TOD.SelectedIndex == 0
                },
                EndTime = new TimeModel
                {
                    Hour = int.Parse(comboEndTime_Hour.Text),
                    Minute = int.Parse(comboEndTime_Min.Text),
                    IsMorning = comboEndTime_TOD.SelectedIndex == 0
                },
                TaskLimit = int.Parse(txtDialyLimit.Text)
            };

            createdSchedule.SetDays(GetDays()); 

            using (var context = new InstaContext())
            {
                var user = context.Users.Include("Schedules").FirstOrDefault(u => u.Id == User.Id);
                user.Schedules.Add(createdSchedule);
                await context.SaveChangesAsync();
                MessageBox.Show("Schedule: " + createdSchedule.Name + " is created.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Reset(); 
            }

            LoadedState(); 
        }

        private List<DayOfWeek> GetDays()
        {
            var list = new List<DayOfWeek>(); 
            
            if(checkEveryDay.IsChecked ?? false)
            {
                list.Add(DayOfWeek.Friday); 
                list.Add(DayOfWeek.Saturday); 
                list.Add(DayOfWeek.Sunday); 
                list.Add(DayOfWeek.Monday);
                list.Add(DayOfWeek.Tuesday);
                list.Add(DayOfWeek.Wednesday); 
                list.Add(DayOfWeek.Thursday);
            }
            else
            {
                if (checkFriday.IsChecked ?? false) list.Add(DayOfWeek.Friday);

                if (checkSaturday.IsChecked ?? false) list.Add(DayOfWeek.Saturday);

                if (checkSunday.IsChecked ?? false) list.Add(DayOfWeek.Sunday);

                if (checkMonday.IsChecked ?? false) list.Add(DayOfWeek.Monday);

                if (checkTuseday.IsChecked ?? false) list.Add(DayOfWeek.Tuesday);

                if (checkWednesday.IsChecked ?? false) list.Add(DayOfWeek.Wednesday);

                if (checkThursday.IsChecked ?? false) list.Add(DayOfWeek.Thursday);
            }

            return list; 
        }
    }
}
