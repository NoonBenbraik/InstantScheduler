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
        List<SearchModel> SavedSearches; 

        public TasksView(UserModel user)
        {
            InitializeComponent();
            this.User = user;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Reset(); 
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
            var result = MessageBox.Show("Reset?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning); 
            if (result == MessageBoxResult.Yes)
                Reset(); 
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            btnCreate.IsEnabled = false;
            btnReset.IsEnabled = false;

           try
            {
                using (var context = new InstaContext())
                {
                    var task = new TaskModel
                    {
                        Name = txtName.Text,
                        TaskType = (TaskType)comboTaskType.SelectedItem,
                        Searches = SelectedSearches,
                        Schedule = context.Schedules.FirstOrDefault(sc => sc.Id == (int)comboSchedule.SelectedValue),
                        Repeat = int.Parse(txtRepeatCount.Text)
                    };


                    var user = context.Users.Include("Tasks").FirstOrDefault(u => u.Id == this.User.Id);
                    user.Tasks.Add(task);
                    await context.SaveChangesAsync();
                    MessageBox.Show("Task: " + task.Name + " created successfully.");
                    Reset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }

            btnCreate.IsEnabled = true;
            btnReset.IsEnabled = true;
        }


        private void Reset()
        {
            using (var context = new InstaContext())
            {
                this.User = context.Users.Include("Schedules").Include("Tasks").Include("Searches").FirstOrDefault(u => u.Id == this.User.Id); 
            }

            pnlTasks.Children.Clear();
            this.User.Tasks.ForEach(t => pnlTasks.Children.Add(new TaskItemView(t)));

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
            lstSelectedSearches.DisplayMemberPath = "Name";
            lstSelectedSearches.SelectedValuePath = "Id";

            txtSeachSearches.Text = "";

            this.SavedSearches = this.User.Searches; 

            lstSavedSearches.ItemsSource = null; 
            lstSavedSearches.ItemsSource = this.SavedSearches;
            lstSavedSearches.DisplayMemberPath = "Name";
            lstSavedSearches.SelectedValuePath = "Id";

            txtRepeatCount.Text = "10"; 
        }

        private void TxtSeachSearches_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tempSearchList = this.SavedSearches.Where(s => s.Name.Contains(txtSeachSearches.Text));
            lstSavedSearches.ItemsSource = null;
            lstSavedSearches.ItemsSource = tempSearchList;
            lstSavedSearches.DisplayMemberPath = "Name"; 
        }

        private void LstSavedSearches_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedSearches.Add((SearchModel)lstSavedSearches.SelectedItem);
            SavedSearches.Remove((SearchModel)lstSavedSearches.SelectedItem);

            lstSelectedSearches.ItemsSource = null;
            lstSelectedSearches.ItemsSource = SelectedSearches;
            lstSelectedSearches.DisplayMemberPath = "Name";

            lstSavedSearches.ItemsSource = null;
            lstSavedSearches.ItemsSource = SavedSearches;
            lstSavedSearches.DisplayMemberPath = "Name";
        }

        private void LstSelectedSearches_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SavedSearches.Add((SearchModel)lstSelectedSearches.SelectedItem);
            SelectedSearches.Remove((SearchModel)lstSelectedSearches.SelectedItem);

            lstSelectedSearches.ItemsSource = null;
            lstSelectedSearches.ItemsSource = SelectedSearches;
            lstSelectedSearches.DisplayMemberPath = "Name";

            lstSavedSearches.ItemsSource = null;
            lstSavedSearches.ItemsSource = SavedSearches;
            lstSavedSearches.DisplayMemberPath = "Name";
        }
    }
}
