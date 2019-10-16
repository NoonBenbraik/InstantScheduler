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

namespace InstantScheduler.Windows
{
    /// <summary>
    /// Interaction logic for MessagesWindow.xaml
    /// </summary>
    public partial class MessagesWindow : Window
    {
        IInstaApi Api; 
        public MessagesWindow(IInstaApi api)
        {
            InitializeComponent();
            this.Api = api; 
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var messages = await Api.GetDirectInboxAsync();
            if (messages.Succeeded)
            {
                pnlConversations.Children.Clear();
                foreach (var thread in messages.Value.Inbox.Threads)
                {
                    pnlConversations.Children.Add(new InboxThreadView(thread)); 
                }
            }
            else
            {
                MessageBox.Show(messages.Info.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
    }
}
