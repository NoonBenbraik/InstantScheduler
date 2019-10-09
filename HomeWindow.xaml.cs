using InstantScheduler.Controls;
using InstantScheduler.DAL;
using InstantScheduler.Models;
using InstantScheduler.Windows;
using InstaSharper.API;
using InstaSharper.Classes.Models;
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
using System.Windows.Shapes;

namespace InstantScheduler
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private IInstaApi Api { get; set; }
        private InstaCurrentUser CurrentUser { get; set; }
        private UserModel UserModel; 

        public HomeWindow(IInstaApi api)
        {
            InitializeComponent();
            this.Api = api; 
        }

        private void DisableButtons()
        {
            btnFeed.IsEnabled = false;
            btnProfile.IsEnabled = false;
            btnSchedules.IsEnabled = false;
            btnSearches.IsEnabled = false;
            btnTasks.IsEnabled = false;
            btnMessages.IsEnabled = false;
            btnSearch.IsEnabled = false;
            txtSearchHere.IsEnabled = false; 
        }

        private void EnableButtons()
        {
            btnFeed.IsEnabled = true;
            btnProfile.IsEnabled = true;
            btnSchedules.IsEnabled = true;
            btnSearches.IsEnabled = true;
            btnTasks.IsEnabled = true;
            btnMessages.IsEnabled = true;
            btnSearch.IsEnabled = true;
            txtSearchHere.IsEnabled = true;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Home - Feed";
            DisableButtons(); 
            var _user = await Api.GetCurrentUserAsync(); 
            if (_user.Succeeded)
            {
                
                this.CurrentUser = _user.Value;

                using (var context = new InstaContext())
                {
                    if(!context.Users.Any(u => u.PK == this.CurrentUser.Pk))
                    {
                        var user = new UserModel
                        {
                            PK = this.CurrentUser.Pk,
                            LastLogin = DateTime.Now
                        };

                        context.Users.Add(user);
                        context.SaveChanges(); 
                    }

                    this.UserModel = context.Users.Include("Schedules").Include("Searches").Include("Tasks").First(u => this.CurrentUser.Pk == u.PK);
                    this.UserModel.LastLogin = DateTime.Now;
                    context.SaveChanges();
                }

                TaskWorker.StartBackgroundWorker(this.UserModel.Id, this.Api); 

                EnableButtons(); 

                profileImage.Fill = new ImageBrush(new BitmapImage(new Uri(this.CurrentUser.ProfilePicture)));
                lblUsername.Content = this.CurrentUser.UserName;
                progressBar.Visibility = Visibility.Hidden; 
            }
            else
            {
                MessageBox.Show(_user.Info.Message); 
                this.Close(); 
            }

            pnlMainContent.Children.Clear();
            pnlMainContent.Children.Add(new FeedView(this.Api)); 
        }


        private void btnFeed_Click(object sender, RoutedEventArgs e)
        {
            this.Title = "Home - Feed";
            pnlMainContent.Children.Clear();
            pnlMainContent.Children.Add(new FeedView(this.Api));

        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            this.Title = "Home - My Profile";
            pnlMainContent.Children.Clear();
            pnlMainContent.Children.Add(new ProfileView(this.Api, this.UserModel.PK));

        }

        private void btnSchedules_Click(object sender, RoutedEventArgs e)
        {
            this.Title = "Home - Schedules";
            pnlMainContent.Children.Clear();
            pnlMainContent.Children.Add(new SchedulesView(this.UserModel));

        }

        private void btnSearches_Click(object sender, RoutedEventArgs e)
        {
            this.Title = "Home - Searches";
            pnlMainContent.Children.Clear();
            pnlMainContent.Children.Add(new SearchesView(this.UserModel, this.Api));

        }

        private void btnTasks_Click(object sender, RoutedEventArgs e)
        {
            this.Title = "Home - Tasks";
            pnlMainContent.Children.Clear();
            pnlMainContent.Children.Add(new TasksView(this.UserModel));

        }

        private void btnMessages_Click(object sender, RoutedEventArgs e)
        {
            new MessagesWindow().Show(); 
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
