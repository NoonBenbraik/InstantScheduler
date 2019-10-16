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
    /// Interaction logic for InboxThreadView.xaml
    /// </summary>
    public partial class InboxThreadView : UserControl
    {
        InstaDirectInboxThread thread; 
        public InboxThreadView(InstaDirectInboxThread thread)
        {
            InitializeComponent();
            this.thread = thread; 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(thread != null)
            {
                lblTitle.Content = thread.Title;
                if(thread.Users.Count > 0)
                    profileImage.Fill = new ImageBrush(new BitmapImage(new Uri(thread.Users.First().ProfilePicture)));
            }
        }
    }
}
