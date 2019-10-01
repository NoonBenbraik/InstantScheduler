using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Logger;
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

namespace InstantScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserSessionData user;
        private IInstaApi api; 

        public MainWindow()
        {
            InitializeComponent();
            user = new UserSessionData(); 
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            txtPassword.IsEnabled = false;
            txtUsername.IsEnabled = false;

            await Login();

            progressBar.Visibility = Visibility.Hidden;
            txtPassword.IsEnabled = true;
            txtUsername.IsEnabled = true;

        }

        private void Home_Closed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }

        private async Task Login()
        {
            user.UserName = txtUsername.Text;
            user.Password = txtPassword.Password;

            this.api = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .UseLogger(new DebugLogger(LogLevel.All))
                .Build();

            var loginReq = await this.api.LoginAsync();

            if (loginReq.Succeeded)
            {
                var home = new HomeWindow(api);
                home.Width = this.Width;
                home.Height = this.Height;
                home.Left = this.Left;
                home.Top = this.Top; 
                home.Closed += Home_Closed;
                home.Show();

                this.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show(loginReq.Info.Message); 
            }
        }

        private bool CheckFields()
        {
            return txtUsername.Text.Count() > 0 && txtPassword.Password.Count() > 0; 
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckFields()) btnLogin.IsEnabled = true;
            else btnLogin.IsEnabled = false;
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (CheckFields()) btnLogin.IsEnabled = true;
            else btnLogin.IsEnabled = false; 
        }
    }
}
