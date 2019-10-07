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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InstantScheduler.Controls
{
    /// <summary>
    /// Interaction logic for PostView.xaml
    /// </summary>
    public partial class PostView : UserControl
    {
        private InstaMedia Media;
        IInstaApi Api; 

        public PostView(InstaMedia media, IInstaApi Api)
        {
            InitializeComponent();
            this.Media = media;
            this.Api = Api; 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.lblUsername.Content = this.Media.User.UserName;
            this.lblDisplayName.Content = this.Media.User.FullName;
            this.profileImage.Fill = new ImageBrush(new BitmapImage(new Uri(this.Media.User.ProfilePicture)));
            //this.lblCreatedAt.Content = this.Media.Caption.CreatedAt.ToString("YYYY-mm-dd");

            this.pnlImages.Children.Clear(); 

            this.Media.Images.ForEach(image =>
            {
                pnlImages.Children.Add(new Image
                {
                    Source = new BitmapImage(new Uri(image.URI)),
                    Height = image.Height,
                    Width = image.Width
                }); 
            });

            if (this.Media.Caption != null)
                this.txtCaption.Text = this.Media.Caption.Text;
            else
                this.txtCaption.Text = ""; 

            this.lblLikeCount.Content = this.Media.LikesCount;

            pnlComments.Children.Clear(); 

            this.Media.PreviewComments.ForEach(c =>
            {
                pnlComments.Children.Add(new CommentView(c, this.Api));
            }); 
        }

        private void BtnProfilePic_Click(object sender, RoutedEventArgs e)
        {
            var profileView = new ViewWindows();
            profileView.pnlViewable.Children.Add(new ProfileView(this.Api, this.Media.User.Pk));
            profileView.ShowDialog(); 
        }
    }
}
