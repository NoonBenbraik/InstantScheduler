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
    /// Interaction logic for SearchesView.xaml
    /// </summary>
    public partial class SearchesView : UserControl
    {
        UserModel User;
        List<Location> ExLocations;
        List<Location> InLocations;
        List<string> ExTexts;
        List<string> InTexts; 

        public SearchesView(UserModel user)
        {
            InitializeComponent();
            this.User = user;

            Reset(); 
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
                this.User = context.Users.Include("Schedules").Include("Searches").Include("Tasks").FirstOrDefault(u => u.Id == this.User.Id); 
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
            ExTexts.Remove((string)lstTextExclude.SelectedItem);
            lstTextExclude.ItemsSource = null;
            lstTextExclude.ItemsSource = ExTexts;
            txtSearchText.Text = "";
        }
    }
}
