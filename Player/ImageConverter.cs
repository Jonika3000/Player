using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Player
{
    public static class ImageConverter  
    {
       
        public static byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            try
            {
                MemoryStream memStream = new MemoryStream();
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(imageC));
                encoder.Save(memStream);
                return memStream.ToArray();
            }
            catch
            {

             }
            return null;
        }
        public static Image ConvertByteArrayToBitmapImage(Byte[] bytes)
        {
            if(bytes != null)
            {
                var stream = new MemoryStream(bytes);
                stream.Seek(0, SeekOrigin.Begin);
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();
                Image image1 = new Image();
                image1.Source = image;
                return image1;
            }
            return null;
        }
    }

}
