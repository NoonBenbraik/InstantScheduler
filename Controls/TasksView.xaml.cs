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
    /// Interaction logic for TasksView.xaml
    /// </summary>
    public partial class TasksView : UserControl
    {
        UserModel User;
        List<SearchModel> SelectedSearches;

        public TasksView(UserModel user)
        {
            InitializeComponent();
            this.User = user;
            Reset();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            pnlTasks.Children.Clear();

            this.User.Tasks.ForEach(t => pnlTasks.Children.Add(new TaskItemView(t))); 
        }

        private void txtRepeatCount_TextChanged(object sender, TextChangedEventArgs e)
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

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            Reset(); 
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            btnCreate.IsEnabled = false;
            btnReset.IsEnabled = false;
            await Create();
            Reset();
            btnCreate.IsEnabled = true;
            btnReset.IsEnabled = true;
        }

        private async Task Create()
        {
            var task = new TaskModel
            {
                Name = txtName.Text,
                TaskType = (TaskType)comboTaskType.SelectedItem,
                Searches = SelectedSearches,
                Schedule = (ScheduleModel)comboSchedule.SelectedItem,
                Repeat = int.Parse(txtRepeatCount.Text)
            };

            using (var context = new InstaContext())
            {
                this.User.Tasks.Add(task);
                await context.SaveChangesAsync();
            }
        }
        private void Reset()
        {
            using (var context = new InstaContext())
            {
                this.User = context.Users.Include("Schedules").Include("Tasks").Include("Searches").FirstOrDefault(u => u.Id == this.User.Id); 
            }

            txtName.Text = "";
            comboTaskType.ItemsSource = Enum.GetValues(typeof(TaskType)).Cast<TaskType>(); ; /* Check this */

            if (this.User.Schedules.Count > 0)
            {
                comboSchedule.ItemsSource = this.User.Schedules;
                comboSchedule.DisplayMemberPath = "Name";
                comboSchedule.SelectedValuePath = "Id"; 
                comboSchedule.SelectedIndex = 0;
            }

            SelectedSearches = new List<SearchModel>();
            lstSelectedSearches.ItemsSource = null;
            lstSelectedSearches.ItemsSource = SelectedSearches;
            lstSelectedSearches.ItemsSource = this.User.Searches;
            lstSelectedSearches.DisplayMemberPath = "Name";
            lstSelectedSearches.SelectedValuePath = "Id";

            txtSeachSearches.Text = ""; 

            lstSavedSearches.ItemsSource = null; 
            lstSavedSearches.ItemsSource = this.User.Searches;
            lstSavedSearches.DisplayMemberPath = "Name";
            lstSavedSearches.SelectedValuePath = "Id";

            txtRepeatCount.Text = "10"; 
        }
    }
}
