using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Player.Pages
{
    /// <summary>
    /// Логика взаимодействия для Setings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        Path p = new Path();
        MainWindow w = Window.GetWindow(Application.Current.MainWindow) as MainWindow;
        public Settings()
        {
            InitializeComponent();
            pathtextbox.Text = p.path;
            SetStatic();
        }
        private void SetStatic()
        { 
            SongsCount.Text = w.songs.Count.ToString();
            PlaylistCount.Text = w.playLists.Count.ToString();
        }
        private void Git_Click(object sender, RoutedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = "https://github.com/Jonika3000?tab=repositories";
            p.Start();
        }


        private void pathtextbox_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            pathtextbox.Text = p.path;
            w.GetSongsFromDirectory();
        }
    }
}
