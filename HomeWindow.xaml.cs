using InstantScheduler.Controls;
using InstaSharper.API;
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
        public HomeWindow(IInstaApi API)
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pnlMainContent.Children.Clear();
            pnlMainContent.Children.Add(new FeedView()); 
        }

        private void btnFeed_Click(object sender, RoutedEventArgs e)
        {
            pnlMainContent.Children.Clear();
            pnlMainContent.Children.Add(new FeedView());

        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            pnlMainContent.Children.Clear();
            pnlMainContent.Children.Add(new ProfileView());

        }

        private void btnSchedules_Click(object sender, RoutedEventArgs e)
        {
            pnlMainContent.Children.Clear();
            pnlMainContent.Children.Add(new SchedulesView());

        }

        private void btnSearches_Click(object sender, RoutedEventArgs e)
        {
            pnlMainContent.Children.Clear();
            pnlMainContent.Children.Add(new SearchesView());

        }

        private void btnTasks_Click(object sender, RoutedEventArgs e)
        {
            pnlMainContent.Children.Clear();
            pnlMainContent.Children.Add(new TasksView());

        }

        private void btnMessages_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
