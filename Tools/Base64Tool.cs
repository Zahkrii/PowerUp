using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PowerUp.Tools
{
    internal class Base64Tool
    {
        private static Base64Tool instance;

        public static Base64Tool Instance
        {
            get
            {
                if (instance == null)
                    instance = new Base64Tool();
                return instance;
            }
        }

        public ImageSource Base64ToImage(string base64string)
        {
            byte[] b = Convert.FromBase64String(base64string);
            MemoryStream ms = new MemoryStream(b);
            Bitmap bitmap = new Bitmap(ms);
            ms.Close();
            BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return bs;
        }

        public string ImageToBase64(ImageSource image)
        {
            string strbaser64 = "";
            try
            {
                //Convert the ImageSource to BitmapSource to be able to Convert to Bitmap.
                BitmapSource bitmapSource = image as BitmapSource;

                //Create a Bitmap from the BitmapSource. The Bitmap will be converted to an array
                Bitmap bitmap;
                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();

                    enc.Frames.Add(BitmapFrame.Create(bitmapSource));
                    enc.Save(outStream);
                    bitmap = new Bitmap(outStream);
                }

                //To prevent the bm.Save() from generating the error "A generic error occurred in GDI+",
                //a new bitmap needs to be created based on the original bitmap.
                Bitmap bm = new Bitmap(bitmap);
                MemoryStream stream = new MemoryStream();
                bm.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imgBytes = stream.ToArray();
                stream.Close();

                strbaser64 = Convert.ToBase64String(imgBytes);
            }
            catch (Exception)
            {
                throw new Exception("Something wrong during convert!");
            }
            return strbaser64;
        }
    }
}