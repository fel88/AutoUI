using AutoUI.TestItems.Editors;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [TestItemEditor(Editor = typeof(SearchByPatternEditor))]
    [XmlParse(XmlKey = "searchPattern")]
    public class SearchByPatternImage : AutoTestItem
    {
        public PatternMatchingImage Pattern;

        public bool ClickOnSucceseed { get; set; }

        public string PatternName { get => Pattern.Name; }
        public override void ParseXml(TestSet parent, XElement item)
        {
            //var data = Convert.FromBase64String(item.Value);
            //MemoryStream ms = new MemoryStream(data);
            //Pattern = Bitmap.FromStream(ms) as Bitmap;

            if (item.Attribute("clickOnSucceseed") != null)
                ClickOnSucceseed = bool.Parse(item.Attribute("clickOnSucceseed").Value);

            var pId = int.Parse(item.Attribute("patternId").Value);
            var p = parent.Pool.Patterns.First(z => z.Id == pId);
            Pattern = p;
            base.ParseXml(parent, item);
        }

        public bool NextSearch { get; set; }
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
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
                Point? ret = null;
                foreach (var item in Pattern.Items)
                {
                    ret = searchPattern(item.Bitmap, startX, startY);
                    if (ret != null)
                    {
                        break;
                    }
                }
                //var ret = searchPattern(startX, startY);
                ctx.LastSearchPosition = ret;
            }
            else
            {
                Point? ret = null;
                foreach (var item in Pattern.Items)
                {
                    ret = searchPattern(item.Bitmap);
                    if (ret != null)
                    {
                        break;
                    }
                }
                if (ret == null)
                {
                    return TestItemProcessResultEnum.Failed;
                }
                ctx.LastSearchPosition = ret;
            }
            if (ClickOnSucceseed)
            {
                var cc = new ClickAutoTestItem();
                cc.Process(ctx);
            }
            return TestItemProcessResultEnum.Success;
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
        Point? searchPattern(Bitmap pattern, int startX = 0, int startY = 0)
        {
            var scrs = GetScreenshot();


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
            //Pattern.Save(ms, ImageFormat.Png);
            //var b64 = Convert.ToBase64String(ms.ToArray());
            return $"<searchPattern patternId=\"{Pattern.Id}\" clickOnSucceseed=\"{ClickOnSucceseed}\" ></searchPattern>";
        }

        public bool Assert { get; set; }
    }
}