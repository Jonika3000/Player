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

namespace Player.Pages
{
    /// <summary>
    /// Логика взаимодействия для RecentlyPage.xaml
    /// </summary>
    public partial class RecentlyPage : Page
    {
        bool gray = true;
        public RecentlyPage()
        {
            InitializeComponent();
            //GetTracks();
        }
        private void GetTracks()
        {
            var w = Window.GetWindow(App.Current.MainWindow) as MainWindow;
            MainStackPanel.Children.Clear();
            for(int i = w.RecentlySongs.Count-1; i>=0 ; i--)
            {
               AddSongToStackPanel.SetStackPanelSongs(MainStackPanel,w.RecentlySongs[i] , ref gray);
            }
        
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetTracks();
        }
    }
}
