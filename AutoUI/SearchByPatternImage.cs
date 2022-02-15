using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AutoUI
{
    [XmlParse(XmlKey = "searchPattern")]
    public class SearchByPatternImage : AutoTestItem
    {
        public Bitmap Pattern;

        public override void ParseXml(XElement item)
        {
            var data = Convert.FromBase64String(item.Value);
            MemoryStream ms = new MemoryStream(data);
            Pattern = Bitmap.FromStream(ms) as Bitmap;

            base.ParseXml(item);
        }

        public bool NextSearch { get; set; }
        public override void Process(AutoTestRunContext ctx)
        {
            if (NextSearch)
            {
                int startX = 0;
                int startY = 0;
                if (ctx.LastSearchPosition != null)
                {
                    startX = ctx.LastSearchPosition.Value.X;
                    startY = ctx.LastSearchPosition.Value.Y + 1;
                }
                var ret = searchPattern(startX, startY);
                ctx.LastSearchPosition = ret;
            }
            else
            {
                var ret = searchPattern();
                ctx.LastSearchPosition = ret;
            }
        }
        public Bitmap GetScreenshot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }

            return bitmap;
        }
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
        private bool IsPixelsEqual(Color px, Color px2)
        {
            return px.R == px2.R && px.G == px2.G && px.B == px2.B;
        }
        Point? searchPattern(int startX = 0, int startY = 0)
        {
            var scrs = GetScreenshot();
            var pattern = Pattern;

            DirectBitmap d = new DirectBitmap(scrs);
            DirectBitmap d2 = new DirectBitmap(pattern);
            int cursor = 0;
            Stopwatch sw = Stopwatch.StartNew();

            Random r = new Random();
            //slide window
            for (int i = startX; i < d.Width - pattern.Width; i++)
            {
                for (int j = startY; j < d.Height - pattern.Height; j++)
                {
                    bool good = true;
                    //pre check random pixels
                    for (int t = 0; t < 10; t++)
                    {
                        var rx = r.Next(pattern.Width);
                        var ry = r.Next(pattern.Height);
                        var px = d.GetPixel(i + rx, j + ry);
                        var px2 = d2.GetPixel(rx, ry);
                        if (!IsPixelsEqual(px, px2)) { good = false; break; }
                    }

                    if (!good) continue;
                    //check pattern match

                    for (int i1 = 0; i1 < pattern.Width; i1++)
                    {
                        for (int j1 = 0; j1 < pattern.Height; j1++)
                        {
                            var px = d.GetPixel(i + i1, j + j1);
                            var px2 = d2.GetPixel(i1, j1);
                            if (!IsPixelsEqual(px, px2)) { good = false; break; }
                        }
                        if (!good) break;
                    }

                    if (good)
                    {

                        sw.Stop();
                        SetCursorPos(i + pattern.Width / 2, j + pattern.Height / 2);
                        return new Point(i, j);
                    }
                }
            }

            return null;
        }

        internal override string ToXml()
        {
            MemoryStream ms = new MemoryStream();
            Pattern.Save(ms, ImageFormat.Png);
            var b64 = Convert.ToBase64String(ms.ToArray());
            return $"<searchPattern>{b64}</searchPattern>";
        }
    }
}