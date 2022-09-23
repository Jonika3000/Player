using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Player
{
    public static class GetAllSongs
    {
       
        public static List<Song> FullList()
        {
            List<Song> songs = new List<Song>();
            List<string> paths = GetMyFiles(new List<string> { "*.mp3", "*.mp2", "*.avi", "*.wav" });
            for (int i = 0; i < paths.Count; i++)
            {

                Song song = SetSong(paths[i]);
                songs.Add(song);

            }
            return songs;
        }
        public static List<string> GetMyFiles(List<string> files)
        {
            List<string> lst = new List<string>();
            Path path = new Path();
            foreach (var ext in files)
            {
                lst.AddRange(Directory.GetFiles(path.path, ext, SearchOption.TopDirectoryOnly).ToList());
            }
            return lst;
        }
    
        private static Song SetSong(string url)
        {
            Song song = new Song();
            song.FullPath = url;
            song.Name = GetName(url);
            song.Time = GetTime(url);
            song.Artists = GetAuthor(url);
            song.Album = GetAlbum(url);
            song.image = new Image();
            song.image.Source = GetImage(url).Source;
            song.Genre = GetGenre(url);
            return song;
        }
        private static string GetGenre(string url)
        {
            TagLib.File file = TagLib.File.Create(url);
            var Genre = file.Tag.FirstGenre;
            return Genre;
        }
        private static Image GetImage(string url)
        {
            TagLib.File file = TagLib.File.Create(url);
            if (file.Tag.Pictures.Length > 0)
            {
                TagLib.IPicture pic = file.Tag.Pictures[0];
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                img.Source = bitmap;
                return img;
            }
            else
            {
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                img.Source = new BitmapImage(new Uri("Image_For_Page/black-music-note-icon-5.png", UriKind.Relative));
                return img;
            }
        }
        private static string GetAlbum(string url)
        {
            TagLib.File file = TagLib.File.Create(url);
            var Album = file.Tag.Album;

            return Album;
        }
        private static string[] GetAuthor(string url)
        {
            TagLib.File file = TagLib.File.Create(url);
            var Artists = file.Tag.Artists;
            return Artists;
        }
        private static string GetName(string url)
        {
            TagLib.File file = TagLib.File.Create(url);
            string title;
            if (file.Tag.Title == null)
            {
                title = System.IO.Path.GetFileNameWithoutExtension(url);
            }
            else
                title = file.Tag.Title;
            return title;
        }
        private static TimeSpan GetTime(string url)
        {
            TagLib.File file = TagLib.File.Create(url);
            var duration = file.Properties.Duration;
            return duration;
        }
    }
}
