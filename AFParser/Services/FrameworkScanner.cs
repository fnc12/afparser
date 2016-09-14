using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AFParser.Services
{
    public class FrameworkScanner
    {
        Color triangleBorderMaxColor;
        Color selectedColor;
        List<int> frameworkLinesCenterY = new List<int>();
        int selectedIndex = -1;
        int controlX=-1;

        const int triangleBaseLength = 11;
        const int nearRadius = 5;

        public FrameworkScanner(Color triangleBorderMaxColor, Color selectedColor)
        {
            this.triangleBorderMaxColor = triangleBorderMaxColor;
            this.selectedColor = selectedColor;
        }

        bool colorIsMatching(Color color, Color maxColor)
        {
            return color.R <= maxColor.R && color.G <= maxColor.G && color.B <= maxColor.B;
        }

        bool colorIsNear(Color color, Color target)
        {
            return Math.Abs(target.R - color.R) < nearRadius
                && Math.Abs(target.G - color.G) < nearRadius
                && Math.Abs(target.B - color.B) < nearRadius;
        }

        int findSelectedIndex(Bitmap bitmap, List<int> frameworkLinesCenterY)
        {
            for(var i=0;i<frameworkLinesCenterY.Count;++i){
                var frameworkLineCenterY=frameworkLinesCenterY[i];
                var controlPixelColor = bitmap.GetPixel(controlX + 14, frameworkLineCenterY);
                if (colorIsNear(controlPixelColor,selectedColor))
                {
                    return i;
                }
            }
            return -1;
        }

        bool findTriangle(Bitmap bitmap, Point origin)
        {
            if (origin.Y + triangleBaseLength < bitmap.Height)
            {
                for (var i = 0; i < triangleBaseLength; ++i)
                {
                    var pixelColor = bitmap.GetPixel(origin.X, origin.Y + i);
                    if (!colorIsMatching(pixelColor, triangleBorderMaxColor))
                    {
                        return false;
                    }
                }
                for (var i = 0; i < 5; ++i)
                {
                    var pixelColor = bitmap.GetPixel(origin.X + 1 + i, origin.Y + 1 + i);
                    if (!colorIsMatching(pixelColor, triangleBorderMaxColor))
                    {
                        return false;
                    }
                }
                for (var i = 0; i < 4; ++i)
                {
                    var pixelColor = bitmap.GetPixel(origin.X + 1 + i, origin.Y + triangleBaseLength - i-2);
                    if (!colorIsMatching(pixelColor, triangleBorderMaxColor))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public int ControlX
        {
            get
            {
                return controlX;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
        }

        public List<int> FrameworkLinesCenterY
        {
            get
            {
                return frameworkLinesCenterY;
            }
        }

        public void Scan(Bitmap bitmap, Rectangle bounds)
        {
            frameworkLinesCenterY.Clear();
            var xMax=bounds.X+bounds.Width;
            var yMax = bounds.Y + bounds.Height;
            for (var x = bounds.X; x < xMax; ++x)
            {
                for (var y = bounds.Y; y < yMax; ++y)
                {
                    var pixelColor = bitmap.GetPixel(x, y);
                    if (colorIsMatching(pixelColor,triangleBorderMaxColor))
                    {
                        if (findTriangle(bitmap,new Point(x,y)))
                        {
                            frameworkLinesCenterY.Add(y + triangleBaseLength / 2);
                            y += triangleBaseLength;
                            //x = bounds.X;
                        }
                    }
                }
                if (frameworkLinesCenterY.Count > 0)
                {
                    controlX = x;
                    break;
                }
            }
            selectedIndex = findSelectedIndex(bitmap, frameworkLinesCenterY);
        }
    }
}
