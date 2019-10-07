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
    /// Interaction logic for CommentView.xaml
    /// </summary>
    public partial class CommentView : UserControl
    {
        InstaComment Comment;
        IInstaApi Api; 

        public CommentView(InstaComment comment, IInstaApi Api)
        {
            InitializeComponent();
            this.Comment = comment;
            this.Api = Api; 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.profileImage.Fill = new ImageBrush(new BitmapImage(new Uri(this.Comment.User.ProfilePicture)));
            this.commentString.Text = this.Comment.Text; 
        }

        private void BtnProfilePic_Click(object sender, RoutedEventArgs e)
        {
            var profileView = new ViewWindows();
            profileView.pnlViewable.Children.Add(new ProfileView(this.Api, this.Comment.User.Pk));
            profileView.ShowDialog();

        }
    }
}
