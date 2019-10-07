using InstantScheduler.Meta;
using InstantScheduler.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for InputValuesWindow.xaml
    /// </summary>
    public partial class InputValuesWindow : Window
    {
        public ValuesModel Values { get; set; }

        public InputValuesWindow(ref ValuesModel values, TaskType taskType)
        {
            InitializeComponent();
            this.Values = values;

            switch (taskType)
            {
                case TaskType.Comment:
                    btnAttachImage.IsEnabled = false;
                    btnAttachVideo.IsEnabled = false;
                    pnlImages.IsEnabled = false;
                    pnlVideos.IsEnabled = false;
                    txtCaption.IsEnabled = true;
                    break;
                case TaskType.Post:
                    btnAttachImage.IsEnabled = true;
                    btnAttachVideo.IsEnabled = true;
                    pnlImages.IsEnabled = true;
                    pnlVideos.IsEnabled = true;
                    txtCaption.IsEnabled = true;
                    break;
                case TaskType.DM:
                    btnAttachImage.IsEnabled = true;
                    btnAttachVideo.IsEnabled = false;
                    pnlImages.IsEnabled = true;
                    pnlVideos.IsEnabled = false;
                    txtCaption.IsEnabled = true;
                    break;
                case TaskType.Follow:
                case TaskType.Unfollow:
                case TaskType.Like:
                case TaskType.Unlike:
                    btnAttachImage.IsEnabled = false;
                    btnAttachVideo.IsEnabled = false;
                    pnlImages.IsEnabled = false;
                    pnlVideos.IsEnabled = false;
                    txtCaption.IsEnabled = false;
                    break;
            }

            this.Title = "Add values: " + taskType.ToString() + " task";

            txtCaption.TextChanged += TxtCaption_TextChanged; 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtCaption.Text = this.Values.Text;
            PopulatePreviews();
        }

        string imagesDirectory = @"Uploads\img\";
        string videosDirectory = @"Uploads\vid\"; 
        private void btnAttachImage_Click(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Multiselect = true; 
            openFile.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
            openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); 

            if(openFile.ShowDialog() == true)
            {
                openFile.FileNames.ToList().ForEach(fileName =>
                {
                    if (!Directory.Exists(imagesDirectory))
                        Directory.CreateDirectory(imagesDirectory);

                    File.Copy(fileName, imagesDirectory + System.IO.Path.GetFileName(fileName), true);

                    Values.Images.Add(new InstaSharp.Models.Image
                    {
                        StandardResolution = new InstaSharp.Models.Resolution
                        {
                            Url = System.IO.Path.GetFullPath(imagesDirectory) + System.IO.Path.GetFileName(fileName)
                        }
                    }); 
                }); 
            }

            PopulatePreviews();
        }

        private void BtnAttachVideo_Click(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Multiselect = true;
            openFile.Filter = "Video files (*.mp4;)|*.mp4;|All files (*.*)|*.*";
            openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            openFile.FileNames.ToList().ForEach(fileName =>
            {
                if (!Directory.Exists(videosDirectory))
                    Directory.CreateDirectory(videosDirectory);

                if (!ValidVideoFile(fileName))
                {
                    MessageBox.Show(System.IO.Path.GetFileName(fileName) + ", can't be uploaded because it vaulates Instagrams limits.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error); 
                }
                else
                {

                    File.Copy(fileName, videosDirectory + System.IO.Path.GetFileName(fileName), true);

                    Values.Videos.Add(new InstaSharp.Models.Video
                    {
                        StandardResolution = new InstaSharp.Models.Resolution
                        {
                            Url = System.IO.Path.GetFullPath(videosDirectory) + System.IO.Path.GetFileName(fileName)
                        }
                    });
                }
            });

            PopulatePreviews(); 
        }

        private void PopulatePreviews()
        {
            pnlImages.Children.Clear(); 
            this.Values.Images.ForEach(image =>
            {
                pnlImages.Children.Add(new MediaElement
                {
                    Source = new Uri(image.StandardResolution.Url),
                    Height = 150,
                    Stretch = Stretch.UniformToFill
                });
            });

            pnlVideos.Children.Clear(); 
            this.Values.Videos.ForEach(video =>
            {
                pnlImages.Children.Add(new MediaElement
                {
                    Source = new Uri(video.StandardResolution.Url),
                    Height = 150,
                    Stretch = Stretch.UniformToFill,
                    IsMuted = true
                });
            });
        }

        private bool ValidVideoFile(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            bool valid = true;

            valid &= fileInfo.Length < 15000000;
            valid &= fileInfo.Extension == ".mp4";

            return valid; 
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void TxtCaption_TextChanged(object sender, TextChangedEventArgs e)
        {
            Values.Text = txtCaption.Text; 
        }
    }
}
