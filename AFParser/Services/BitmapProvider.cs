using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace AFParser.Services
{
    public class BitmapProvider
    {
        public BitmapProvider()
        {
            
        }

        Bitmap makeScreenshot()
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                           Screen.PrimaryScreen.Bounds.Height,
                                           PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            /*bmpScreenshot.Save("Screenshot.png", ImageFormat.Png);

            var color = bmpScreenshot.GetPixel(0, 0);
            Console.WriteLine("color = {0}", color);*/
            return bmpScreenshot;
        }

        public Bitmap GetBitmap()
        {
            //return new Bitmap(@"C:\Documents and Settings\Администратор\Рабочий стол\alex.png");
            //return new Bitmap(@"bitmap2.png");
            return makeScreenshot();
        }
    }
}
