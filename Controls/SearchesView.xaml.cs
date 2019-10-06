using InstantScheduler.DAL;
using InstantScheduler.Models;
using InstaSharper.API;
using Newtonsoft.Json;
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
    /// Interaction logic for SearchesView.xaml
    /// </summary>
    public partial class SearchesView : UserControl
    {
        UserModel User;
        public List<Location> ExLocations;
        public List<Location> InLocations;
        List<string> ExTexts;
        List<string> InTexts;
        IInstaApi Api;
        public SearchesView(UserModel user, IInstaApi Api)
        {
            InitializeComponent();
            this.User = user;
            this.Api = Api;

            Reset();

            txtSearchLocation.TextChanged += TxtSearchLocation_TextChanged; 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            pnlSearches.Children.Clear(); 

            this.User.Searches.ForEach(s => pnlSearches.Children.Add(new SearchItemView(s))); 
        }

        private void Reset()
        {
            using (var context = new InstaContext())
            {
                this.User = context.Users.Include("Searches").Include("Schedules").Include("Tasks").FirstOrDefault(u => u.Id == this.User.Id); 
            }

            pnlSearches.Children.Clear();
            this.User.Searches.ForEach(s => pnlSearches.Children.Add(new SearchItemView(s)));

            txtName.Text = "";
            txtSearchText.Text = "";
            txtSearchLocation.Text = "";

            InLocations = new List<Location>();
            lstLocationInclude.ItemsSource = null;
            lstLocationInclude.ItemsSource = InLocations; 

            ExLocations = new List<Location>();
            lstLocationExclude.ItemsSource = null;
            lstLocationExclude.ItemsSource = ExLocations; 

            InTexts = new List<string>();
            lstTextInclude.ItemsSource = null;
            lstTextInclude.ItemsSource = InTexts; 

            ExTexts = new List<string>();
            lstTextExclude.ItemsSource = null;
            lstTextExclude.ItemsSource = ExTexts;

            checkInUsers.IsChecked = false;
            checkInHashtags.IsChecked = false;
            checkInPosts.IsChecked = false;
            checkInFollowingMe.IsChecked = false; 
        
        }

        private void BtnTextInclude_Click(object sender, RoutedEventArgs e)
        {
            InTexts.Add(txtSearchText.Text);
            lstTextInclude.ItemsSource = null;
            lstTextInclude.ItemsSource = InTexts;
            txtSearchText.Text = ""; 
        }

        private void BtnTextExclude_Click(object sender, RoutedEventArgs e)
        {
            ExTexts.Add(txtSearchText.Text);
            lstTextExclude.ItemsSource = null;
            lstTextExclude.ItemsSource = ExTexts;
            txtSearchText.Text = "";
        }

        private void LstTextInclude_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            InTexts.Remove((string)lstTextInclude.SelectedItem);
            lstTextInclude.ItemsSource = null;
            lstTextInclude.ItemsSource = InTexts;
            txtSearchText.Text = "";
        }

        private void LstTextExclude_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ExTexts.Remove((string)lstTextExclude.SelectedItem);
            lstTextExclude.ItemsSource = null;
            lstTextExclude.ItemsSource = ExTexts;
            txtSearchText.Text = "";
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Reset?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                Reset(); 
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var createdSearch = new SearchModel
                {
                    Name = txtName.Text,
                    InStrings = JsonConvert.SerializeObject(this.InTexts),
                    ExStrings = JsonConvert.SerializeObject(this.ExTexts),
                    InPosts = checkInPosts.IsChecked ?? false,
                    InHashtags = checkInHashtags.IsChecked ?? false,
                    InUsers = checkInUsers.IsChecked ?? false,
                    InFollowingMe = checkInFollowingMe.IsChecked ?? false,
                    StartDate = dateStart.SelectedDate ?? DateTime.Now,
                    EndDate = dateStart.SelectedDate ?? DateTime.Now
                };

                createdSearch.SetInLocations(InLocations);
                createdSearch.SetExLocations(ExLocations); 

                using (var context = new InstaContext())
                {
                    var user = context.Users.Include("Searches").FirstOrDefault(u => u.Id == this.User.Id);
                    user.Searches.Add(createdSearch); 
                    await context.SaveChangesAsync();
                    MessageBox.Show("Search: " + createdSearch.Name + " created Successfully.");
                    Reset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        private void BtnLocationExclude_Click(object sender, RoutedEventArgs e)
        {
            new LocationSearchWindow(this, false, txtSearchLocation.Text, this.Api).ShowDialog(); 
        }

        private void BtnLocationInclude_Click(object sender, RoutedEventArgs e)
        {
            new LocationSearchWindow(this, true, txtSearchLocation.Text, this.Api).ShowDialog();
        }

        private void TxtSearchLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtSearchLocation.Text.Count() > 0)
            {
                btnLocationInclude.IsEnabled = true;
                btnLocationExclude.IsEnabled = true;
            }
            else
            {
                btnLocationInclude.IsEnabled = false;
                btnLocationExclude.IsEnabled = false;
            }
        }

    }
}
