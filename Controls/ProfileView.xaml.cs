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
using InstaSharper;
using InstaSharper.API;
using InstaSharper.Classes;

namespace InstantScheduler.Controls
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControl
    {
        IInstaApi Api;
        long PK;
        public ProfileView(IInstaApi api, long PK)
        {
            InitializeComponent();
            this.Api = api;
            this.PK = PK;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var user = await Api.GetUserInfoByIdAsync(this.PK); 

            if (user.Succeeded)
            {
                profileImage.Fill = new ImageBrush(new BitmapImage(new Uri(user.Value.ProfilePicUrl)));
                lblDisplayName.Content = user.Value.FullName;
                lblUserName.Content = user.Value.Username;
                lblPostCount.Content = user.Value.MediaCount;
                lblFollowingCount.Content = user.Value.FollowingCount; 
                lblFollowersCount.Content = user.Value.FollowerCount;
                textBiography.Text = user.Value.Biography; 

                var posts = await Api.GetUserMediaAsync(user.Value.Username, PaginationParameters.MaxPagesToLoad(1));

                if (posts.Succeeded)
                {
                    posts.Value.ForEach(p => pnlPosts.Children.Add(new PostView(p))); 
                }

            }
        }
    }
}
