using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AFParser.Services
{
    public class RecordParser
    {
        enum Direction
        {
            Right, Down, Left, Up,
        }

        Rectangle recordFormFrame;
        Color[] textBoxFrameColors = null;
        KeyboardService keyboardService = null;
        TextParser textParser = null;
        List<Rectangle> textBoxesRectangles = new List<Rectangle>();

        const int textBoxWidthMin = 4;
        const int textBoxHeightMin = 4;

        public RecordParser(Rectangle recordFormFrame, Color[] textBoxFrameColors, KeyboardService keyboardService, TextParser textParser)
        {
            this.recordFormFrame = recordFormFrame;
            this.textBoxFrameColors = textBoxFrameColors;
            this.keyboardService = keyboardService;
            this.textParser = textParser;
        }

        bool colorExistsInArray(Color color, Color[] array)
        {
            /*foreach (var c in array)
            {
                if (c.Equals(color))
                {
                    return true;
                }
            }
            return false;*/
            return color.R >= array[0].R && color.R <= array[1].R
                && color.G >= array[0].G && color.G <= array[1].G
                && color.B >= array[0].B && color.B <= array[1].B;
        }

        Rectangle findTextBox(Bitmap bitmap, int x, int y, Color[] textBoxBorderColors)
        {
            var res = new Rectangle();
            res.X = x;
            res.Y = y;
            var direction = Direction.Right;
            do
            {
                var pixelColor = bitmap.GetPixel(x, y);
                switch (direction)
                {
                    case Direction.Right:
                        if (colorExistsInArray(pixelColor, textBoxBorderColors))
                        {
                            x++;
                        }
                        else
                        {
                            res.Width = x - res.X;
                            if (res.Width > textBoxWidthMin)
                            {
                                x = res.X;
                                direction = Direction.Down;
                            }
                            else
                            {
                                throw new Exception("Width too small");
                            }
                        }
                        break;
                    case Direction.Down:
                        if (colorExistsInArray(pixelColor, textBoxBorderColors))
                        {
                            ++y;
                        }
                        else
                        {
                            res.Height = y - res.Y;
                            if (res.Height > textBoxHeightMin)
                            {
                                return res;
                            }
                            else
                            {
                                throw new Exception("Height too small");
                            }
                        }
                        break;
                }
            } while (true);
        }

        List<Rectangle> findTextBoxes(Bitmap bitmap, Rectangle boundsToSearch, Color[] textBoxBorderColors)
        {
            var res = new List<Rectangle>();
            var boxIsInFocus = false;
            var yMax = boundsToSearch.Y + boundsToSearch.Height;
            var xMax = boundsToSearch.X + boundsToSearch.Width;
            for (var y = boundsToSearch.Y; y < yMax; ++y)
            {
                for (var x = boundsToSearch.X; x < xMax; ++x)
                {
                    var pixelColor = bitmap.GetPixel(x, y);
                    if (!boxIsInFocus)
                    {
                        if (colorExistsInArray(pixelColor, textBoxBorderColors))
                        {
                            try
                            {
                                var textBoxBounds = findTextBox(bitmap, x, y, textBoxBorderColors);
                                res.Add(textBoxBounds);
                                //Console.WriteLine("textBoxBounds = {0}", textBoxBounds);
                                x = boundsToSearch.X;
                                y = textBoxBounds.Y + textBoxBounds.Height + 1;
                            }
                            catch (Exception)
                            {
                                //Console.WriteLine("ex = {0}", ex);
                                x = boundsToSearch.X;
                                ++y;
                            }
                        }
                    }
                }
            }
            return res;
        }

        public string GetTextBoxValue(Rectangle textBoxRect)
        {
            var nullString = "<null>";
            //Clipboard.SetText(nullString);
            var mousePoint = new MouseOperations.MousePoint(textBoxRect.X + textBoxRect.Width / 2, textBoxRect.Y + textBoxRect.Height / 2);
            MouseOperations.LeftMouseClick(mousePoint);
            MouseOperations.LeftMouseClick(mousePoint);
            Thread.Sleep(500);
            keyboardService.CtrlA();
            Thread.Sleep(500);
            keyboardService.CtrlC();
            Thread.Sleep(1500);
            var res=Clipboard.GetText();
            if (res != nullString)
            {
                return res;
            }
            else
            {
                return null;
            }
        }

        public void FindTextBoxes(Bitmap bitmap)
        {
            textBoxesRectangles = findTextBoxes(bitmap, recordFormFrame, textBoxFrameColors);
            Console.WriteLine("textBoxesRectangles[{0}] = [", textBoxesRectangles.Count);
            foreach (var textBoxesRectangle in textBoxesRectangles)
            {
                Console.WriteLine("\t{0}", textBoxesRectangle);
            }
            Console.WriteLine("]");
        }

        public string GetValue(string caption,Bitmap bitmap,out int index,int previousIndex)
        {
            caption = caption.Replace(" ", "");
            //makeScreenshot();
            //var bitmap = getBitmap();
            //bitmap.Save("bitmap.bmp");
            /*if (textBoxesRectangles.Count == 0)
            {
                FindTextBoxes(bitmap);
            }*/

            //var titleIndex = -1;
            index = -1;
            for (var i = previousIndex; i < textBoxesRectangles.Count; ++i)
            {
                var textBoxesRectangle = textBoxesRectangles[i];
                var titleRect = new Rectangle(
                    recordFormFrame.X + 1,
                    textBoxesRectangle.Y,
                    textBoxesRectangle.X - recordFormFrame.X - 1,
                    textBoxesRectangle.Height-3);
                Console.WriteLine("titleRect[{0}] = {1}", i, titleRect);
                var tm = textParser.titleMatches(bitmap, titleRect, caption);
                if (tm)
                {
                    Console.WriteLine("title matches at {0}", i);
                    index = i;
                    break;
                }
                else
                {
                    Console.WriteLine("doesn't match at {0}", i);
                }
            }
            if (index > -1)
            {
                //MessageBox.Show("Title index = " + titleIndex.ToString() + ", rect = " + textBoxesRectangles[titleIndex].ToString());
                var title = GetTextBoxValue(textBoxesRectangles[index]);
                //MessageBox.Show("Title = *" + title + "*");
                return title;
            }
            else
            {
                //MessageBox.Show("titleIndex is -1");
                return null;
            }
        }
    }
}
