using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Player
{
    [Serializable]
    public class Song
    {
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public TimeSpan Time { get; set; }
        public string[] Artists { get; set; }
        public Image image { get; set; }
        public string lastopenedplace { get; set; } 
         public string Album { get; set; }
        public Song(string fullPath, string name, string genre, TimeSpan time, string[] artist, Image image, string album)
        {
            FullPath = fullPath;
            Name = name;
            Genre = genre;
            Time = time;
            Artists = artist;
            this.image = image;
            Album = album;
        }
        public Song()
            {

            }
    }
}
