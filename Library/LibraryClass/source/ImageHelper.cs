using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace LibraryClass
{
    public static class ImageHelper
    {
        public static BitmapSource BytesToImage(byte[] imageData)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    var decoder = BitmapDecoder.Create(ms,
                        BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                    return decoder.Frames[0];
                }
            }
            catch (Exception e)
            {
                return null;
            }
           
        }

        public static byte[] ImageToBytes(BitmapImage imageC)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {

                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(imageC));
                    encoder.Save(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception e)
            {
                return null;
            }
           
        }
    }
}
