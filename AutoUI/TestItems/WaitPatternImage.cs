using AutoUI.TestItems.Editors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [TestItemEditor(Editor = typeof(WaitPatternEditor))]
    [XmlParse(XmlKey = "waitPattern")]
    public class WaitPatternImage : AutoTestItem
    {
        public List<PatternMatchingImage> Patterns = new List<PatternMatchingImage>();


        public override void ParseXml(TestSet parent, XElement item)
        {
            var pIds = item.Attribute("patternIds").Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            var pns = pIds.Select(zz => parent.Pool.Patterns.First(z => z.Id == zz)).ToList();
            Patterns = pns;
            base.ParseXml(parent, item);
        }


        public int Timeout { get; set; } = 100 * 1000;
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            Stopwatch start = Stopwatch.StartNew();
            Point? ret = null;
            while (true)
            {
                if (start.Elapsed.TotalMilliseconds > Timeout)
                {
                    return TestItemProcessResultEnum.Failed;
                }
                var screen = GetScreenshot();
                foreach (var pattern in Patterns)
                {
                    foreach (var item in pattern.Items)
                    {
                        ret = searchPattern(screen, item.Bitmap);
                        if (ret != null)
                        {
                            break;
                        }
                    }
                    if (ret != null) return TestItemProcessResultEnum.Success;
                }
                screen.Dispose();
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
        
        private bool IsPixelsEqual(Color px, Color px2)
        {
            return px.R == px2.R && px.G == px2.G && px.B == px2.B;
        }

        Point? searchPattern(Bitmap scrs, Bitmap pattern, int startX = 0, int startY = 0)
        {
            DirectBitmap d = new DirectBitmap(scrs);
            DirectBitmap d2 = new DirectBitmap(pattern);
            
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
                        d.Dispose();
                        d2.Dispose();
                        return new Point(i, j);
                    }
                }
            }
            d.Dispose();
            d2.Dispose();

            return null;
        }

        internal override string ToXml()
        {
            return $"<waitPattern patternIds=\"{string.Join(";", Patterns.Select(z => z.Id).ToArray())}\"  ></waitPattern>";
        }

        public bool Assert { get; set; }
    }
}