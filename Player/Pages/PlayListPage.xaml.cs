using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Player.Pages
{
    /// <summary>
    /// Логика взаимодействия для PlayListPage.xaml
    /// </summary>
    public partial class PlayListPage : Page
    {
        MainWindow w = Window.GetWindow(App.Current.MainWindow) as MainWindow;
        bool gray = true;
        PlayList playListCurrent = new PlayList();
        string prev = string.Empty;
        public PlayListPage()
        {
            InitializeComponent();
            SetNameNewPlayList();
            SetImageNewPlayList();
            w.playLists.Add(playListCurrent);
            CreateButtonPlayList();
        }
        public PlayListPage(PlayList playList)
        {
            InitializeComponent();
            playListCurrent = playList;
            SetPlayList();
        }
        private void CreateButtonPlayList()
        {
            w.ListPlaylist.Children.Add(AddSongToStackPanel.CreateButtonPlayList(playListCurrent));

        }
        public void UpdateList()
        {
            MainStackPanel.Children.Clear();
            foreach (var s in playListCurrent.Songs)
            {
                AddSongToStackPanel.SetStackPanelSongs(MainStackPanel, s, ref gray);
            }
        }


        private void SetPlayList()
        {
            PlayListImage.Source = playListCurrent.image.Source;
            PlayListName.Text = playListCurrent.Name;
            foreach (var s in playListCurrent.Songs)
            {
                AddSongToStackPanel.SetStackPanelSongs(MainStackPanel, s, ref gray);
            }
        }
        private void PlayListImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                PlayListImage.Source = new BitmapImage(new Uri(op.FileName));
                playListCurrent.image.Source = new BitmapImage(new Uri(op.FileName));
                UpdateButton();
            }
        }
        private void SetNameNewPlayList()
        {
            playListCurrent.Name = "Playlist" + $" #{w.playLists.Count}";
            PlayListName.Text = playListCurrent.Name;
        }
        private void SetImageNewPlayList()
        {
            playListCurrent.image.Source = new BitmapImage(new Uri("/Pages/Image_for_page/black-music-note-icon-5.png", UriKind.RelativeOrAbsolute));
            PlayListImage.Source = new BitmapImage(new Uri("/Pages/Image_for_page/black-music-note-icon-5.png", UriKind.RelativeOrAbsolute));
        }


        private void UpdateButton()
        {
            foreach (Button button in w.ListPlaylist.Children)
            {
                if (button.Tag == playListCurrent.Name)
                {
                    ((Image)(((StackPanel)button.Content).Children[0])).Source = playListCurrent.image.Source;
                    //((TextBlock)(((StackPanel)button.Content).Children[1])).Text = "   " + PlayListName.Text;

                    break;
                }
            }
        }

        private void PlayListName_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                PlayListName.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
        private void UpdateLastPlayedPlaceSong(string old, string _new)
        {
            w.songs.Where(c => c.lastopenedplace == old).Select(c => { c.lastopenedplace = _new; return c; }).ToList();
        }
        private void PlayListName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var b = w.playLists.Where(a => a.Name == PlayListName.Text).FirstOrDefault();
            if (b == null)
            {
                foreach (var item in w.playLists)
                {
                    if (item.Name == playListCurrent.Name)
                    {
                        foreach (Button button in w.ListPlaylist.Children)
                        {
                            if (button.Tag == playListCurrent.Name)
                            {
                                UpdateLastPlayedPlaceSong(playListCurrent.Name, PlayListName.Text);
                                button.Tag = PlayListName.Text;
                                ((Image)(((StackPanel)button.Content).Children[0])).Source = playListCurrent.image.Source;
                                ((TextBlock)(((StackPanel)button.Content).Children[1])).Text = "   " + PlayListName.Text;

                                break;
                            }
                        }
                        item.Name = PlayListName.Text;
                        playListCurrent.Name = PlayListName.Text;
                        UpdateButton();
                        break;
                    }
                }

            }

        }
    }
}
