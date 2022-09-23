using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Player
{
    public class Path
    {
        public  string path { get; set; } 

        public Path()
        {
            try
            {
                path = JsonSerializer.Deserialize<string>("path.json");

            }
            catch
            {
                if (path == null || path == string.Empty)
                {
                    path = @"C:\Users\Home\Music";
                }
            }
            
        }
        private void ChangePathJson()
        {
            File.Delete("path.json");
            using (FileStream fs = new FileStream("path.json", FileMode.Create))
            {
                JsonSerializer.Serialize<string>(fs, path);
            }
        }
    }
}
