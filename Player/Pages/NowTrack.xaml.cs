using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Player.Pages
{
    /// <summary>
    /// Логика взаимодействия для NowTrack.xaml
    /// </summary>
    public partial class NowTrack : Page
    {
        MainWindow w = Window.GetWindow(App.Current.MainWindow) as MainWindow;

        public NowTrack()
        {
            InitializeComponent();
        }
        public NowTrack(Song song)
        {
            InitializeComponent();
            SetTrack(song);
            SetTrackNext(song);
            SetTrackPrev(song);
        }
        private void SetTrack(Song song)
        {
            TrackImage.Source = song.image.Source;
            if (song.Name.Length > 26)
            {
                string tmp = song.Name.Substring(0, 20);
                tmp = tmp + "...";
                TrackName.Text = tmp;
            }
            else
                TrackName.Text = song.Name;
            if (song.Artists.Count() == 0)
                TrackArtist.Text = "Unknown artist";
            else
                TrackArtist.Text = string.Join(" ", song.Artists);
            if (song.Album == null || song.Album == string.Empty)
                TrackAlbum.Text = "Unknown album";
            else
                TrackAlbum.Text = song.Album;
        }
        private void SetTrackPrev(Song song)
        {
            for (int i = 0; i < w.songs.Count; i++)
            {
                if (w.songs[i].FullPath == song.FullPath)
                {
                    if (w.songs[i].lastopenedplace != "Page_AllMusic"
                       && w.songs[i].lastopenedplace != "RecentlyPage"
                       && w.songs[i].lastopenedplace != "Search"
                        && w.songs[i].lastopenedplace != "ArtistSongs")
                    {
                        if (w.songs[i].FullPath == song.FullPath)
                        {
                            var tmpList = w.playLists.Where(q => q.Name == w.songs[i].lastopenedplace).FirstOrDefault();
                            for (int j = 0; j < tmpList.Songs.Count; j++)
                            {
                                if (tmpList.Songs[j].Name == w.songs[i].Name)
                                {
                                    if (j > 0)
                                    {
                                        j--;
                                    }
                                    else
                                    {
                                        j = tmpList.Songs.Count - 1;
                                    }
                                    TrackImagePrev.Source = tmpList.Songs[j].image.Source;
                                    if (tmpList.Songs[j].Name.Length > 20)
                                    {
                                        string tmp = tmpList.Songs[j].Name.Substring(0, 20);
                                        tmp = tmp + "...";
                                        TrackNamePrev.Text = tmp;
                                    }
                                    else
                                        TrackNamePrev.Text = tmpList.Songs[j].Name;
                                    if (tmpList.Songs[j].Artists.Count() == 0)
                                        TrackArtistPrev.Text = "Unknown artist";
                                    else
                                        TrackArtistPrev.Text = string.Join(" ", tmpList.Songs[j].Artists);
                                    if (tmpList.Songs[j].Album == null || tmpList.Songs[j].Album == string.Empty)
                                        TrackAlbumPrev.Text = "Unknown album";
                                    else
                                        TrackAlbumPrev.Text = tmpList.Songs[j].Album;
                                    break;
                                }
                            }
                        }
                    }
                    else if (w.songs[i].lastopenedplace == "ArtistSongs")
                    {
                        var tmpList = w.songs.Where(q => string.Join(" ", q.Artists) == string.Join(" ", w.songs[i].Artists)).ToList();
                        if (tmpList.Count == 0)
                        {
                            tmpList = w.songs.Where(q => string.Join(" ", q.Artists) == string.Empty).ToList();
                        }

                        for (int j = 0; j < tmpList.Count; j++)
                        {
                            if (tmpList[j].FullPath == w.songs[i].FullPath)
                            {
                                if (j > 0)
                                {
                                    j--;
                                }
                                else
                                {
                                    j = tmpList.Count - 1;
                                }
                                TrackImagePrev.Source = tmpList[j].image.Source;
                                if (tmpList[j].Name.Length > 20)
                                {
                                    string tmp = tmpList[j].Name.Substring(0, 20);
                                    tmp = tmp + "...";
                                    TrackNamePrev.Text = tmp;
                                }
                                else
                                    TrackNamePrev.Text = tmpList[j].Name;
                                if (tmpList[j].Artists.Count() == 0)
                                    TrackArtistPrev.Text = "Unknown artist";
                                else
                                    TrackArtistPrev.Text = string.Join(" ", tmpList[j].Artists);
                                if (tmpList[j].Album == null || tmpList[j].Album == string.Empty)
                                    TrackAlbumPrev.Text = "Unknown album";
                                else
                                    TrackAlbumPrev.Text = tmpList[j].Album;
                                return;
                            }
                        }

                    }
                    else
                    {
                        if (i > 0)
                        {
                            i--;
                        }

                        else
                        {
                            i = w.songs.Count - 1;
                        }

                        if (i < w.songs.Count && i >= 0)
                        {
                            TrackImagePrev.Source = w.songs[i].image.Source;
                            if (w.songs[i].Name.Length > 20)
                            {
                                string tmp = w.songs[i].Name.Substring(0, 20);
                                tmp = tmp + "...";
                                TrackNamePrev.Text = tmp;
                            }
                            else
                                TrackNamePrev.Text = w.songs[i].Name;
                            if (w.songs[i].Artists.Count() == 0)
                                TrackArtistPrev.Text = "Unknown artist";
                            else
                                TrackArtistPrev.Text = string.Join(" ", w.songs[i].Artists);
                            if (w.songs[i].Album == null || w.songs[i].Album == string.Empty)
                                TrackAlbumPrev.Text = "Unknown album";
                            else
                                TrackAlbumPrev.Text = w.songs[i].Album;
                            break;
                        }
                    }
                }
            }

        }
        private void SetTrackNext(Song song)
        {
            for (int i = 0; i < w.songs.Count; i++)
            {
                if (w.songs[i].FullPath == song.FullPath)
                {
                    if (w.songs[i].lastopenedplace != "Page_AllMusic"
                       && w.songs[i].lastopenedplace != "RecentlyPage"
                       && w.songs[i].lastopenedplace != "Search"
                        && w.songs[i].lastopenedplace != "ArtistSongs")
                    {
                        if (w.songs[i].FullPath == song.FullPath)
                        {
                            var tmpList = w.playLists.Where(q => q.Name == w.songs[i].lastopenedplace).FirstOrDefault();
                            for (int j = 0; j < tmpList.Songs.Count; j++)
                            {
                                if (tmpList.Songs[j].Name == w.songs[i].Name)
                                {
                                    if (j < tmpList.Songs.Count - 1)
                                    {
                                        j++;
                                    }
                                    else
                                    {
                                        j = 0;
                                    }
                                    TrackImageNext.Source = tmpList.Songs[j].image.Source;
                                    if (tmpList.Songs[j].Name.Length > 20)
                                    {
                                        string tmp = tmpList.Songs[j].Name.Substring(0, 20);
                                        tmp = tmp + "...";
                                        TrackNameNext.Text = tmp;
                                    }
                                    else
                                        TrackNameNext.Text = tmpList.Songs[j].Name;
                                    if (tmpList.Songs[j].Artists.Count() == 0)
                                        TrackArtistNext.Text = "Unknown artist";
                                    else
                                        TrackArtistNext.Text = string.Join(" ", tmpList.Songs[j].Artists);
                                    if (tmpList.Songs[j].Album == null || tmpList.Songs[j].Album == string.Empty)
                                        TrackAlbumNext.Text = "Unknown album";
                                    else
                                        TrackAlbumNext.Text = tmpList.Songs[j].Album;
                                    break;
                                }
                            }
                        }
                    }
                    else if (w.songs[i].lastopenedplace == "ArtistSongs")
                    {
                        var tmpList = w.songs.Where(q => string.Join(" ", q.Artists) == string.Join(" ", w.songs[i].Artists)).ToList();
                        if (tmpList.Count == 0)
                        {
                            tmpList = w.songs.Where(q => string.Join(" ", q.Artists) == string.Empty).ToList();

                        }
                        for (int j = 0; j < tmpList.Count; j++)
                        {
                            if (tmpList[j].FullPath == w.songs[i].FullPath)
                            {
                                if (j < tmpList.Count - 1)
                                    j++;
                                else
                                    j = 0;
                                TrackImageNext.Source = tmpList[j].image.Source;
                                if (tmpList[j].Name.Length > 20)
                                {
                                    string tmp = tmpList[j].Name.Substring(0, 20);
                                    tmp = tmp + "...";
                                    TrackNameNext.Text = tmp;
                                }
                                else
                                    TrackNameNext.Text = tmpList[j].Name;
                                if (tmpList[j].Artists.Count() == 0)
                                    TrackArtistNext.Text = "Unknown artist";
                                else
                                    TrackArtistNext.Text = string.Join(" ", tmpList[j].Artists);
                                if (tmpList[j].Album == null || tmpList[j].Album == string.Empty)
                                    TrackAlbumNext.Text = "Unknown album";
                                else
                                    TrackAlbumNext.Text = tmpList[j].Album;
                                return;
                            }
                        }



                    }
                    else
                    {
                        if (i < w.songs.Count - 1)
                            i++;
                        else
                            i = 0;
                        if (i < w.songs.Count && i >= 0)
                        {
                            TrackImageNext.Source = w.songs[i].image.Source;
                            if (w.songs[i].Name.Length > 20)
                            {
                                string tmp = w.songs[i].Name.Substring(0, 20);
                                tmp = tmp + "...";
                                TrackNameNext.Text = tmp;
                            }
                            else
                                TrackNameNext.Text = w.songs[i].Name;
                            if (w.songs[i].Artists.Count() == 0)
                                TrackArtistNext.Text = "Unknown artist";
                            else
                                TrackArtistNext.Text = string.Join(" ", w.songs[i].Artists);
                            if (w.songs[i].Album == null || w.songs[i].Album == string.Empty)
                                TrackAlbumNext.Text = "Unknown album";
                            else
                                TrackAlbumNext.Text = w.songs[i].Album;
                            break;
                        }
                    }
                }

            }
        }
    }
}
