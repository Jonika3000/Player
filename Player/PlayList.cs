using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Player
{
    public class PlayList
    {
        public string Name;
        public Image image = new Image();
        public List<Song> Songs { get; set; }
        public PlayList()
            {
            Songs = new List<Song>();
            }
    }
}
