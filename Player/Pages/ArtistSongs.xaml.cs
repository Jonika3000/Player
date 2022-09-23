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
    /// Логика взаимодействия для ArtistSongs.xaml
    /// </summary>
    public partial class ArtistSongs : Page
    {
        string searchartist = string.Empty;
        bool gray = true;
        public ArtistSongs()
        {
            InitializeComponent();
        }
        public ArtistSongs(string name)
        {
            InitializeComponent();
            searchartist = name;
            ArtistName.Text = name; 
            GetSongs();
        }
        private void GetSongs()
        {
             var w = Window.GetWindow(App.Current.MainWindow) as MainWindow;
            var tmplst = w.songs.Where(q => string.Join(" ", q.Artists) == searchartist).ToList();
            if( tmplst.Count ==0)
            {
                tmplst = w.songs.Where(q => string.Join(" ", q.Artists) == string.Empty).ToList();
            }
            foreach(var song in tmplst)
            {
                AddSongToStackPanel.SetStackPanelSongs(MainStackPanel, song,ref gray);
            }
        }
    }
}
