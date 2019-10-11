using InstaSharper.API;
using InstaSharper.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InstantScheduler.Controls
{
    /// <summary>
    /// Interaction logic for FeedView.xaml
    /// </summary>
    public partial class FeedView : UserControl
    {
        private IInstaApi Api { get; set; }
        private InstaMediaList Feed { get; set; }

        public FeedView(IInstaApi api)
        {
            InitializeComponent();
            this.Api = api;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var feed = await this.Api.GetLikeFeedAsync(PaginationParameters.MaxPagesToLoad(1));

                if (feed.Succeeded)
                {
                    Feed = feed.Value;

                    Feed.ForEach(m =>
                    {
                        pnlPosts.Children.Add(new PostView(m, Api));
                    });
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
