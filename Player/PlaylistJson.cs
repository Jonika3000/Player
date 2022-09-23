using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    [Serializable]
    public class PlaylistJson
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public List<string> Songs   { get; set; } 
    }
}
