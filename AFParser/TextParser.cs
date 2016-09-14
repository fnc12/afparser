using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

//using Mask = List<List<bool>>;

namespace AFParser
{
    public class TextParser
    {
        Dictionary<char, List<List<bool>>> masks = new Dictionary<char, List<List<bool>>>();

        static Color textColor = Color.FromArgb(0, 0, 0);

        public TextParser()
        {

        }

        List<List<bool>> loadMask(char c)
        {
            var res = new List<List<bool>>();
            var filename = "";
            if (Char.IsLower(c))
            {
                filename += c;
            }
            else
            {
                filename += c + "_cap";
            }
            filename += ".txt";
            using (var sr = File.OpenText("letter_masks/"+filename))
            {
                var s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.Length > 0)
                    {
                        var line = new List<bool>(s.Length);
                        foreach (var ch in s)
                        {
                            if (ch == '1')
                            {
                                line.Add(true);
                            }
                            else
                            {
                                line.Add(false);
                            }
                        }
                        res.Add(line);
                    }
                }
            }
            return res;
        }

        List<List<bool>> getOrLoadMask(char c)
        {
            List<List<bool>> mask = null;
            masks.TryGetValue(c, out mask);
            if (null == mask)
            {
                mask = loadMask(c);
                masks.Add(c, mask);
            }
            return mask;
        }

        bool charMatches(Bitmap bitmap, List<List<bool>> mask, Point origin, Point limit)
        {
            var originOffset = new Point(0, 0);
            for (var i = 0; i < mask.Count; ++i)
            {
                if (!mask[i][0])
                {
                    ++originOffset.Y;
                }
                else
                {
                    break;
                }
            }
            origin.Y -= originOffset.Y;
            var maskSize = new Size(mask[0].Count, mask.Count);
            var opositeOrigin = new Point(origin.X + maskSize.Width, origin.Y + maskSize.Height);
            if (opositeOrigin.X < limit.X && opositeOrigin.Y < limit.Y)
            {
                //var res = true;
                for (var y = origin.Y; y < opositeOrigin.Y; ++y)
                {
                    for (var x = origin.X; x < opositeOrigin.X; ++x)
                    {
                        var pixelColor = bitmap.GetPixel(x, y);
                        var maskValue = mask[y - origin.Y][x - origin.X];
                        if (maskValue)
                        {
                            if (!pixelColor.Equals(textColor))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            /*if (pixelColor.Equals(textColor))
                            {
                                return false;
                            }*/
                        }
                    }
                }
                return true;
            }
            else
            {
                //  no room for mask..
                return false;
            }
        }

        public bool titleMatches(Bitmap bitmap, Rectangle boundsToSearch,string title)
        {
            var pos = boundsToSearch.Location;
            var limit = new Point(
                boundsToSearch.X + boundsToSearch.Width, 
                boundsToSearch.Y + boundsToSearch.Height);
            var foundString = "";
            foreach (var c in title)
            {
                var mask = getOrLoadMask(c);
                var breakFlag=false;
                for (; pos.X < limit.X; ++pos.X)
                {
                    for (; pos.Y < limit.Y; ++pos.Y)
                    {
                        var pixelColor = bitmap.GetPixel(pos.X, pos.Y);
                        if (pixelColor.Equals(textColor))
                        {
                            if (charMatches(bitmap, mask, pos, limit))
                            {
                                Console.WriteLine("*{0}* found at {1}", c,pos);
                                pos.X += mask[0].Count;
                                breakFlag = true;
                                foundString += c;
                                break;
                            }else{
                                return false;
                                //break;
                            }
                        }
                    }
                    pos.Y = boundsToSearch.Y;
                    if (breakFlag)
                    {
                        break;
                    }
                }
            }
            return foundString==title;
        }

    }
}
