using InstantScheduler.Controls;
using InstantScheduler.Models;
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
    /// Interaction logic for LocationSearchWindow.xaml
    /// </summary>
    public partial class LocationSearchWindow : Window
    {
        SearchesView _Parent;
        bool Include = false;
        IInstaApi Api;
        string SearchString; 

        public LocationSearchWindow(SearchesView parent, bool include, string searchString, IInstaApi Api)
        {
            InitializeComponent();
            this._Parent = parent;
            this.Include = include;
            this.Api = Api;
            this.SearchString = searchString; 
        }

        private void lstLocations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.Include)
            {
                this._Parent.InLocations.Add((Location)this.lstLocations.SelectedItem);
                this._Parent.lstLocationInclude.ItemsSource = null;
                this._Parent.lstLocationInclude.ItemsSource = this._Parent.InLocations;
                this._Parent.lstLocationInclude.DisplayMemberPath = "Title";
                
            }
            else
            {
                this._Parent.ExLocations.Add((Location)this.lstLocations.SelectedItem);
                this._Parent.lstLocationExclude.ItemsSource = null;
                this._Parent.lstLocationExclude.ItemsSource = this._Parent.ExLocations;
                this._Parent.lstLocationExclude.DisplayMemberPath = "Title";
            }

            this._Parent.txtSearchLocation.Text = "";
            this.Close(); 
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var results = await Api.SearchPlace(SearchString, 25); 

            if (results.Succeeded)
            {
                var places = results.Value;
                List<Location> Locations = new List<Location>();
                places.Items.ToList().ForEach(p => Locations.Add(new Location
                {
                    Title = p.Title, 
                    FbLocation = p.Location
                }));

                lstLocations.ItemsSource = null;
                lstLocations.ItemsSource = Locations;
                lstLocations.DisplayMemberPath = "Title"; 
                progressBar.Visibility = Visibility.Collapsed; 
            }
            else
            {
                MessageBox.Show("Something Went wrong.");
                this.Close(); 
            }

        }
    }
}
