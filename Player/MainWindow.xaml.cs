using Player.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public DispatcherTimer _timer = null;
        public DispatcherTimer _timerName = null;
        bool IsDragging = false;
        public List<string> Genre = new List<string>();
        bool pause = false;
        string Namebefore;
        public List<PlayList> playLists = new List<PlayList>();
        public List<Song> songs = new List<Song>();

        public List<Song> RecentlySongs = new List<Song>();
        // In your constructor/loaded method

        public MainWindow()
        {
            InitializeComponent();
            GetSongsFromDirectory();
            Container.Navigate(new System.Uri("Pages/Page_AllMusic.xaml", UriKind.RelativeOrAbsolute));

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += _timer_tick;

            _timerName = new DispatcherTimer();
            _timerName.Interval = TimeSpan.FromMilliseconds(100);
            _timerName.Tick += _timerName_Tick;

            VolumeSlider.Value = 100;

        }
        public void GetSongsFromDirectory()
        {
            songs = GetAllSongs.FullList();
        }
        private void ClearLastOpened()
        {
            foreach (var song in songs)
            {
                song.lastopenedplace = string.Empty;
            }
            foreach (var song in RecentlySongs)
            {
                song.lastopenedplace = string.Empty;
            }
        }
        private async void _timerName_Tick(object? sender, EventArgs e)
        {
            if (TrackName.Text.Length > 15)
            {
                TrackName.Text = TrackName.Text.Remove(0, 1);
            }
            else
                TrackName.Text = Namebefore;
        }

        void _timer_tick(object sender, EventArgs e)
        {
            StartTime.Text = string.Format("{0:m\\:ss}", TimeSpan.FromSeconds(meMedia1.Position.TotalSeconds).Duration());
            if (!IsDragging && !pause)
            {
                PlayProgress.Value = meMedia1.Position.TotalSeconds;
            }
            if (meMedia1.NaturalDuration.HasTimeSpan)
            {
                if (meMedia1.Position == meMedia1.NaturalDuration.TimeSpan)
                {
                    PlayNextSong();
                }
            }

        }
        private void Button_Click_Stop(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Button_Click_Max(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void Button_Clicl_Minimize(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void AllButton_Click(object sender, RoutedEventArgs e)
        {
            Container.Navigate(new System.Uri("Pages/Page_AllMusic.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Tg_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Tg_Btn.IsChecked == true)
            {
                tmpName.Visibility = Visibility.Hidden;
                SearchTermTextBox.Text = string.Empty;
                ImageSearch.Visibility = Visibility.Visible;
            }
            else
            {

                tmpName.Visibility = Visibility.Visible;
                ImageSearch.Visibility = Visibility.Hidden;
            }
        }

        private void ImageSearch_Click(object sender, RoutedEventArgs e)
        {
            tmpName.Visibility = Visibility.Visible;
            ImageSearch.Visibility = Visibility.Hidden;
            Tg_Btn.IsChecked = false;
        }
        private void ReadRecentlySongs()
        {
            List<string> tmp = new List<string>();
            try
            {
                using (FileStream fs = new FileStream("RecentlySongs.json", FileMode.OpenOrCreate))
                {
                    tmp = JsonSerializer.Deserialize<List<string>>(fs);
                }
            }
            catch
            {
                File.Delete("RecentlySongs.json");
            }
            foreach (var s in tmp)
            {
                foreach (var item in songs)
                {

                    if (s == item.FullPath)
                    {
                        RecentlySongs.Add(item);
                        break;
                    }
                }
            }
        }
        private void SetStackPanelPlayList()
        {
            foreach (var p in playLists)
            {
                ListPlaylist.Children.Add(AddSongToStackPanel.CreateButtonPlayList(p));
            }
        }
        private void ReadPlaylists()
        {
            List<PlaylistJson> tmp = new List<PlaylistJson>();
            try
            {
                using (FileStream fs = new FileStream("Playlist.json", FileMode.OpenOrCreate))
                {
                    tmp = JsonSerializer.Deserialize<List<PlaylistJson>>(fs);
                }
            }
            catch
            {
                System.IO.File.Delete("Playlist.json");
            }
            foreach (var item in tmp)
            {
                PlayList playlist = new PlayList();
                playlist.Name = item.Name;
                playlist.image = ImageConverter.ConvertByteArrayToBitmapImage(item.Image);
                if (item.Songs != null)
                {
                    foreach (var str in item.Songs)
                    {
                        foreach (var s in songs)
                        {
                            if (str == s.FullPath)
                            {
                                playlist.Songs.Add(s);
                                break;
                            }
                        }
                    }
                }

                playLists.Add(playlist);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReadRecentlySongs();
            ReadPlaylists();
            SetStackPanelPlayList();
            ClearLastOpened();
        }

        private void AddRecentlySong()
        {
            if (RecentlySongs.Count > 50)
            {
                RecentlySongs.Remove(RecentlySongs[RecentlySongs.Count - 1]);

            }
            for (int i = 0; i < songs.Count; i++)
            {
                var u = new Uri(songs[i].FullPath, UriKind.RelativeOrAbsolute);
                if (u == meMedia1.Source)
                {
                    RecentlySongs.Add(songs[i]);
                    break;
                }
            }
        }
        private void meMedia1_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (meMedia1.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = meMedia1.NaturalDuration.TimeSpan;
                PlayProgress.Maximum = ts.TotalSeconds;
                PlayProgress.SmallChange = 1;
                PlayProgress.LargeChange = Math.Min(10, ts.Seconds / 10);
                meMedia1.Volume = (double)VolumeSlider.Value;
                if (TrackName.Text.Length > 15)
                {
                    Namebefore = TrackName.Text;
                    Namebefore = "       " + Namebefore;
                    _timerName.Start();

                }
                Page currentPage = Container.Content as Page;
                if (currentPage != null && currentPage.Title == "NowTrack")
                {
                    SetNowSongPage();
                }
                else
                    AddRecentlySong();
            }
            _timer.Start();
        }

        private void PlayProgress_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            IsDragging = false;
            meMedia1.Position = TimeSpan.FromSeconds(PlayProgress.Value);
        }

        private void PlayProgress_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            IsDragging = true;
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (!pause)
            {
                pause = true;
                meMedia1.Pause();
                PlayImage.Source = new BitmapImage(new Uri("/Image/icons8_play_24px.png", UriKind.Relative));
            }
            else
            {
                pause = false;
                meMedia1.Play();
                PlayImage.Source = new BitmapImage(new Uri("Image/icons8_pause_24px.png", UriKind.Relative));
            }
        }

        private void VolumeSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            meMedia1.Volume = (double)VolumeSlider.Value;
            if (meMedia1.Volume == 0)
            {
                ImageVolume.Source = new BitmapImage(new Uri("/Image/icons8_mute_50px.png", UriKind.Relative));
            }
            else
                ImageVolume.Source = new BitmapImage(new Uri("/Image/icons8_sound_50px.png", UriKind.Relative));
        }

        private void VolumeButton_Click(object sender, RoutedEventArgs e)
        {
            if (meMedia1.Volume > 0)
            {
                VolumeSlider.Value = 0;
                meMedia1.Volume = 0;
                ImageVolume.Source = new BitmapImage(new Uri("/Image/icons8_mute_50px.png", UriKind.Relative));
            }
            else
            {
                VolumeSlider.Value = 1;
                meMedia1.Volume = 1;
                ImageVolume.Source = new BitmapImage(new Uri("/Image/icons8_sound_50px.png", UriKind.Relative));
            }
        }
        private void PlayPrevSong()
        {
            for (int i = 0; i < songs.Count; i++)
            {
                var u = new Uri(songs[i].FullPath, UriKind.RelativeOrAbsolute);
                if (u == meMedia1.Source)
                {
                    if (songs[i].lastopenedplace != "Page_AllMusic"
                        && songs[i].lastopenedplace != "RecentlyPage"
                        && songs[i].lastopenedplace != "Search"
                        && songs[i].lastopenedplace != "ArtistSongs"
                        && songs[i].lastopenedplace != "ArtistSongs")
                    {
                        var tmpList = playLists.Where(q => q.Name == songs[i].lastopenedplace).FirstOrDefault();
                        for (int j = 0; j < tmpList.Songs.Count; j++)
                        {
                            if (tmpList.Songs[j].Name == songs[i].Name)
                            {
                                if (j > 0)
                                {
                                    j--;
                                    foreach (var s in songs)
                                        if (s.Name == tmpList.Songs[j].Name)
                                            s.lastopenedplace = tmpList.Name;
                                    PlaySong(tmpList.Songs[j]);
                                }
                                else
                                {
                                    j = tmpList.Songs.Count - 1;
                                    foreach (var s in songs)
                                        if (s.Name == tmpList.Songs[j].Name)
                                            s.lastopenedplace = tmpList.Name;
                                    PlaySong(tmpList.Songs[j]);

                                }
                                break;
                            }
                        }
                        break;
                    }
                    else if (songs[i].lastopenedplace == "ArtistSongs")
                    {
                        var tmpList = songs.Where(q => string.Join(" ", q.Artists) == string.Join(" ", songs[i].Artists)).ToList();
                        for (int j = 0; j < tmpList.Count; j++)
                        {
                            if (tmpList[j].FullPath == songs[i].FullPath)
                            {
 
                                if (j > 0)
                                {
                                    j--;
                                }
                                else
                                {
                                    j = tmpList.Count - 1;
                                }
                                songs.Where(q => q.FullPath == tmpList[j].FullPath).Select(c => { c.lastopenedplace = "ArtistSongs"; return c; }).ToList();

                                PlaySong(tmpList[j]);
                                break;
                            }
                        }

                    }
                    else
                    {
                        if (i > 0)
                        {
                            songs[i - 1].lastopenedplace = songs[i].lastopenedplace;
                            i--;
                            PlaySong(songs[i]);
                        }
                        else
                        {
                            songs[songs.Count - 1].lastopenedplace = songs[i].lastopenedplace;
                            i = songs.Count - 1;
                            PlaySong(songs[i]);

                        }
                    }
                    _timerName.Stop();

                    break;
                }
            }
        }
        private void PlayNextSong()
        {
            for (int i = 0; i < songs.Count; i++)
            {
                var u = new Uri(songs[i].FullPath, UriKind.RelativeOrAbsolute);
                if (u == meMedia1.Source)
                {
                    if (songs[i].lastopenedplace != "Page_AllMusic"
                        && songs[i].lastopenedplace != "RecentlyPage"
                        && songs[i].lastopenedplace != "Search"
                         && songs[i].lastopenedplace != "ArtistSongs")
                    {
                        var tmpList = playLists.Where(q => q.Name == songs[i].lastopenedplace).FirstOrDefault();
                        for (int j = 0; j < tmpList.Songs.Count; j++)
                        {
                            if (tmpList.Songs[j].Name == songs[i].Name)
                            {
                                if (j < tmpList.Songs.Count - 1)
                                {
                                    j++;
                                    foreach (var s in songs)
                                        if (s.Name == tmpList.Songs[j].Name)
                                            s.lastopenedplace = tmpList.Name;
                                    PlaySong(tmpList.Songs[j]);
                                }
                                else
                                {
                                    j = 0;
                                    foreach (var s in songs)
                                        if (s.Name == tmpList.Songs[j].Name)
                                            s.lastopenedplace = tmpList.Name;
                                    PlaySong(tmpList.Songs[j]);

                                }
                                break;
                            }
                        }
                    }
                    else if (songs[i].lastopenedplace == "ArtistSongs")
                    {
                        var tmpList = songs.Where(q => string.Join(" ", q.Artists) == string.Join(" ", songs[i].Artists)).ToList();
                        if (tmpList.Count > 0)
                        {
                            for (int j = 0; j < tmpList.Count; j++)
                            {
                                if (tmpList[j].FullPath == songs[i].FullPath)
                                {
                                    if (j < tmpList.Count - 1)
                                        j++;
                                    else
                                        j = 0;
                                    //customers.Where(c => c.IsValid).Select(c => { c.CreditLimit = 1000; return c; }).ToList();
                                     songs.Where(q => q.FullPath == tmpList[j].FullPath).Select(c => { c.lastopenedplace = "ArtistSongs"; return c; }).ToList();

                                    PlaySong(tmpList[j]);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (i < songs.Count - 1)
                        {
                            songs[i + 1].lastopenedplace = songs[i].lastopenedplace;
                            i++;
                            PlaySong(songs[i]);
                        }
                        else
                        {
                            songs[songs.Count - 1].lastopenedplace = songs[i].lastopenedplace;
                            i = 0;
                            PlaySong(songs[i]);

                        }
                    }
                    _timerName.Stop();

                    break;
                }
            }
        }
        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            PlayNextSong();

        }
        public void PlaySong(Song song)
        {
            EndTime.Text = string.Format("{0:m\\:ss}", TimeSpan.FromSeconds(song.Time.TotalSeconds).Duration());
            PlayImage.Source = new BitmapImage(new Uri("Image/icons8_pause_24px.png", UriKind.Relative));
            StartTime.Text = "0:00";
            TrackName.Text = song.Name;
            ArtistName.Text = string.Join(" ", song.Artists); ;

            try
            {
                if (song.image.Source.Width > 0)
                {
                    ImageSong.Source = song.image.Source;
                }
            }
            catch
            {
                ImageSong.Source = new BitmapImage(new Uri("/Image/black-music-note-icon-5.png", UriKind.Relative));
            }
            meMedia1.Source = new Uri(song.FullPath);
            meMedia1.Play();
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (meMedia1.Position.TotalSeconds > 5)
            {
                meMedia1.Position = TimeSpan.Zero;
            }
            else
            {
                if (songs.Count > 1)
                {
                    PlayPrevSong();
                }


            }
        }

        private void SearchTermTextBox_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search s = new Search(SearchTermTextBox.Text);
                Container.Navigate(s);
                SearchTermTextBox.Text = String.Empty;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            meMedia1.Volume = (double)VolumeSlider.Value;
        }

        private void NowPlayButton_Click(object sender, RoutedEventArgs e)
        {
            SetNowSongPage();
        }
        private void SetNowSongPage()
        {
            for (int i = songs.Count - 1; i >= 0; i--)
            {
                var u = new Uri(songs[i].FullPath, UriKind.RelativeOrAbsolute);
                if (u == meMedia1.Source)
                {
                    NowTrack nowTrack = new NowTrack(songs[i]);
                    Container.Navigate(nowTrack);
                    break;
                }
            }
        }

        private void ImageSong_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetNowSongPage();
        }

        private void RecentlyButton_Click(object sender, RoutedEventArgs e)
        {
            RecentlyPage recentlyPage = new RecentlyPage();
            Container.Navigate(recentlyPage);
        }
        private void SaveRecentlySongs()
        {
            List<string> tmp = new List<string>();

            foreach (var s in RecentlySongs)
            {
                if (s.Name != null)
                {
                    tmp.Add(s.FullPath);
                }
                else
                    RecentlySongs.Remove(s);
            }

            using (FileStream fs = new FileStream("RecentlySongs.json", FileMode.Create))
            {
                JsonSerializer.Serialize<List<string>>(fs, tmp);

            }
        }
        private void SavePlaylists()
        {
            List<PlaylistJson> json = new List<PlaylistJson>();
            foreach (var playlist in playLists)
            {
                PlaylistJson js = new PlaylistJson();
                js.Songs = new List<string>();
                foreach (var s in playlist.Songs)
                {
                    if (s.Name != null)
                    {
                        js.Songs.Add(s.FullPath);

                    }

                }
                js.Image = ImageConverter.getJPGFromImageControl(playlist.image.Source as BitmapImage);

                js.Name = playlist.Name;
                json.Add(js);
            }
            using (FileStream fs = new FileStream("Playlist.json", FileMode.Create))
            {
                JsonSerializer.Serialize<List<PlaylistJson>>(fs, json);

            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            SaveRecentlySongs();
            SavePlaylists();
        }

        private void CreatePlaylist_Click(object sender, RoutedEventArgs e)
        {
            PlayListPage playListPage = new PlayListPage();
            Container.Navigate(playListPage);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            Container.Navigate(settings);
        }
    }
}
