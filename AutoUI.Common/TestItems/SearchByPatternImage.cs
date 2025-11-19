using AutoUI.Common;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using Point = System.Drawing.Point;

namespace AutoUI.TestItems
{

    [XmlParse(XmlKey = "searchPattern")]
    public class SearchByPatternImage : AutoTestItem
    {
        public PatternMatchingImage Pattern;

        public TestItemProcessResultEnum StatusIfFailed { get; set; } = TestItemProcessResultEnum.Failed;
        public bool ClickIfSucceed { get; set; }
        public bool MouseMoveIfSucceed { get; set; } = true;
        public bool DelayEnabled { get; set; }

        public int Delay { get; set; }

        public string PatternName { get => Pattern == null ? null : Pattern.Name; }

        public override void ParseXml(IAutoTest parent, XElement item)
        {
            if (item.Attribute("clickIfSucceed") != null)
                ClickIfSucceed = bool.Parse(item.Attribute("clickIfSucceed").Value);

            if (item.Attribute("mouseMoveIfSucceed") != null)
                MouseMoveIfSucceed = bool.Parse(item.Attribute("mouseMoveIfSucceed").Value);

            if (item.Attribute("statusIfFailed") != null)
                StatusIfFailed = Enum.Parse<TestItemProcessResultEnum>(item.Attribute("statusIfFailed").Value);

            if (item.Attribute("delay") != null)
                Delay = int.Parse(item.Attribute("delay").Value);

            if (item.Attribute("delayEnabled") != null)
                DelayEnabled = bool.Parse(item.Attribute("delayEnabled").Value);

            if (item.Attribute("preCheck") != null)
                PreCheckCurrentPosition = bool.Parse(item.Attribute("preCheck").Value);

            if (!string.IsNullOrEmpty(item.Attribute("patternId").Value))
            {
                var pId = int.Parse(item.Attribute("patternId").Value);
                var p = parent.Parent.Pool.Patterns.First(z => z.Id == pId);
                Pattern = p;
            }
            base.ParseXml(parent, item);
        }

        public bool PreCheckCurrentPosition { get; set; } = false;

        public bool NextSearch { get; set; }
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            var screen = GetScreenshot();

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
                    int? maxW = null;
                    int? maxH = null;
                    if (PreCheckCurrentPosition)
                    {
                        maxW = item.Bitmap.Width * 2 + 1;
                        maxH = item.Bitmap.Height * 2 + 1;
                        ret = SearchPattern(screen, item, Cursor.Position.X - item.Bitmap.Width, Cursor.Position.Y - item.Bitmap.Height, maxW, maxH);
                    }
                    if (ret == null)
                        ret = SearchPattern(screen, item, startX, startY);
                    if (ret != null)
                    {
                        SetCursorPos(ret.Value.X + item.Bitmap.Width / 2, ret.Value.Y + item.Bitmap.Height / 2);
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
                    int? maxW = null;
                    int? maxH = null;
                    if (PreCheckCurrentPosition)
                    {
                        maxW = item.Bitmap.Width * 2 + 1;
                        maxH = item.Bitmap.Height * 2 + 1;
                        ret = SearchPattern(screen, item, Cursor.Position.X - item.Bitmap.Width, Cursor.Position.Y - item.Bitmap.Height, maxW, maxH);
                    }
                    if (ret == null)
                        ret = SearchPattern(screen, item);
                    if (ret != null)
                    {
                        if (ClickIfSucceed || MouseMoveIfSucceed)
                            SetCursorPos(ret.Value.X + item.Bitmap.Width / 2, ret.Value.Y + item.Bitmap.Height / 2);
                        break;
                    }
                }
                if (ret == null)
                {
                    return StatusIfFailed;
                }
                ctx.LastSearchPosition = ret;
            }

            screen.Dispose();
            if (ClickIfSucceed)
            {
                var cc = new ClickAutoTestItem();
                cc.Process(ctx);
            }

            if (DelayEnabled)
                Thread.Sleep(Delay);

            return TestItemProcessResultEnum.Success;
        }
        public override string ToString()
        {
            return $"search pattern ({PatternName})";
        }

        public static Bitmap GetScreenshot()
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
        public static extern bool SetCursorPos(int X, int Y);

        public static bool IsPixelsEqual(PatternMatchingImageItem pattern, Color px, Color px2)
        {
            switch (pattern.PixelsMode)
            {
                case PixelsMatchingMode.Precise:
                    return px.R == px2.R && px.G == px2.G && px.B == px2.B;

                case PixelsMatchingMode.Distance:
                    return Math.Abs(px.R - px2.R) <= pattern.PixelsMatchDistancePerChannel &&
                        Math.Abs(px.G - px2.G) <= pattern.PixelsMatchDistancePerChannel &&
                        Math.Abs(px.B - px2.B) <= pattern.PixelsMatchDistancePerChannel;

                default:
                    break;
            }
            throw new ArgumentException();
        }

        public static Color ToGrayscale(Color clr)
        {
            var mean = (clr.R + clr.G + clr.B) / 3;
            return Color.FromArgb(mean, mean, mean);
        }
        public static Color ToBinaryMean(Color clr)
        {
            var g = ToGrayscale(clr);
            return g.R > 128 ? Color.White : Color.Black;
        }

        public static Bitmap GetConvertedBitmap(PatternMatchingImageItem _pattern, Bitmap bmp)
        {
            Bitmap ret = new Bitmap(bmp.Width, bmp.Height);
            for (int i = 0; i < ret.Width; i++)
            {
                for (int j = 0; j < ret.Height; j++)
                {
                    ret.SetPixel(i, j, ConvertPixel(_pattern, bmp.GetPixel(i, j)));
                }
            }
            return ret;
        }

        public static Point? SearchPattern(Bitmap screen, PatternMatchingImageItem _pattern, int startX = 0, int startY = 0, int? maxWidth = null, int? maxHeight = null)
        {
            if (_pattern.Mode == PatternMatchingMode.FastBinary)
                return SearchPatternBinary(screen, _pattern, startX, startY, maxWidth, maxHeight);
            if (_pattern.Mode == PatternMatchingMode.TemplateMatching)
                return SearchPatternTM(screen, _pattern, startX, startY, maxWidth, maxHeight);

            /*if (Debugger.IsAttached)
            {
                Helpers.ExecuteSTA(() => { Clipboard.SetImage(GetConvertedBitmap(_pattern, screen)); });

                Helpers.ExecuteSTA(() =>
                {
                    Clipboard.SetImage(GetConvertedBitmap(_pattern, _pattern.Bitmap));
                });
            }*/

            var pattern = _pattern.Bitmap;
            DirectBitmap d = new DirectBitmap(screen);
            DirectBitmap d2 = new DirectBitmap(pattern);

            Stopwatch sw = Stopwatch.StartNew();

            Random r = new Random();
            List<Point> points = new List<Point>();
            List<Color> clrs = new List<Color>();
            var maxErrors = pattern.Width * pattern.Height * (_pattern.PixelsMatchAcceptableErrorLevel / 100.0);

            int previewPixelsQty = (int)(10 + maxErrors);

            for (int t = 0; t < previewPixelsQty; t++)
            {
                var rx = r.Next(pattern.Width);
                var ry = r.Next(pattern.Height);
                points.Add(new Point(rx, ry));
                var px = ConvertPixel(_pattern, d2.GetPixel(rx, ry));

                clrs.Add(px);
            }

            //slide window
            var www = d.Width - pattern.Width;
            var hhh = d.Height - pattern.Height;
            if (maxWidth != null)
                www = Math.Min(maxWidth.Value, www);

            if (maxHeight != null)
                hhh = Math.Min(maxHeight.Value, hhh);

            int errors = 0;
            for (int i = startX; i < www; i++)
            {
                for (int j = startY; j < hhh; j++)
                {
                    startY = 0;
                    bool good = true;
                    //pre check random pixels
                    for (int t = 0; t < points.Count; t++)
                    {
                        var rx = points[t].X;
                        var ry = points[t].Y;
                        var px = d.GetPixel(i + rx, j + ry);
                        px = ConvertPixel(_pattern, px);

                        if (!IsPixelsEqual(_pattern, px, clrs[t]))
                            errors++;

                        if (errors > maxErrors)
                        {
                            errors = 0;
                            good = false;
                            break;
                        }
                    }

                    if (!good)
                        continue;

                    //check pattern match
                    errors = 0;
                    for (int i1 = 0; i1 < pattern.Width; i1++)
                    {
                        for (int j1 = 0; j1 < pattern.Height; j1++)
                        {
                            var px = ConvertPixel(_pattern, d.GetPixel(i + i1, j + j1));
                            var px2 = ConvertPixel(_pattern, d2.GetPixel(i1, j1));

                            if (!IsPixelsEqual(_pattern, px, px2))
                            {
                                errors++;
                            }
                            if (errors > maxErrors)
                            {
                                errors = 0;
                                good = false;
                                break;
                            }
                        }
                        if (!good)
                            break;
                    }

                    if (good)
                    {
                        d.Dispose();
                        d2.Dispose();

                        sw.Stop();

                        return new System.Drawing.Point(i, j);
                    }
                }
            }
            d.Dispose();
            d2.Dispose();

            return null;
        }

        static TemplateMatchInfo[] TemplateMatch(Mat reference, Mat template)
        {
            List<TemplateMatchInfo> matches = new List<TemplateMatchInfo>();
            using Mat refMat = reference.Clone();
            using Mat tplMat = template.Clone();
            using Mat res = new Mat(refMat.Rows - tplMat.Rows + 1, refMat.Cols - tplMat.Cols + 1, MatType.CV_32FC1);

            //Convert input images to gray
            Mat gref = refMat.CvtColor(ColorConversionCodes.BGR2GRAY);
            Mat gtpl = tplMat.CvtColor(ColorConversionCodes.BGR2GRAY);

            Cv2.MatchTemplate(gref, gtpl, res, TemplateMatchModes.CCoeffNormed);
            Cv2.Threshold(res, res, 0.8, 1.0, ThresholdTypes.Tozero);

            while (true)
            {
                double minval, maxval, threshold = 0.8;
                OpenCvSharp.Point minloc, maxloc;
                Cv2.MinMaxLoc(res, out minval, out maxval, out minloc, out maxloc);

                if (maxval >= threshold)
                {
                    //Setup the rectangle to draw
                    Rect r = new Rect(new OpenCvSharp.Point(maxloc.X, maxloc.Y), new OpenCvSharp.Size(tplMat.Width, tplMat.Height));
                    //WriteLine($"MinVal={minval.ToString()} MaxVal={maxval.ToString()} MinLoc={minloc.ToString()} MaxLoc={maxloc.ToString()} Rect={r.ToString()}");
                    matches.Add(new TemplateMatchInfo() { Rect = r, Confidence = maxval });
                    //Draw a rectangle of the matching area
                    //Cv2.Rectangle(refMat, r, Scalar.LimeGreen, 2);

                    //Fill in the res Mat so you don't find the same area again in the MinMaxLoc
                    Rect outRect;
                    Cv2.FloodFill(res, maxloc, new Scalar(0), out outRect, new Scalar(0.1), new Scalar(1.0));
                }
                else
                    break;
            }

            return matches.ToArray();
        }
        public static TemplateMatchInfo[] TemplateMatchingAll(Bitmap screen,
            PatternMatchingImageItem _pattern)
        {
            using var mat1 = screen.ToMat();
            using var mat2 = _pattern.Bitmap.ToMat();
            var matches = TemplateMatch(mat1, mat2);

            return matches.ToArray();

        }

        public static System.Drawing.Point? SearchPatternTM(Bitmap screen, PatternMatchingImageItem _pattern, int startX = 0, int startY = 0, int? maxWidth = null, int? maxHeight = null)
        {
            using var mat1 = screen.ToMat();
            using var mat2 = _pattern.Bitmap.ToMat();
            var matches = TemplateMatch(mat1, mat2);
            if (matches.Any())
                return matches.Take(1).Select(z => new Point(z.Rect.X, z.Rect.Y)).First();
            return null;
        }

        public static System.Drawing.Point? SearchPatternBinary(Bitmap screen, PatternMatchingImageItem _pattern, int startX = 0, int startY = 0, int? maxWidth = null, int? maxHeight = null)
        {
            using var mat1 = screen.ToMat();
            using var d = mat1.Threshold(128, 255, ThresholdTypes.Binary);
            using var mat2 = _pattern.Bitmap.ToMat();
            using var d2 = mat2.Threshold(128, 255, ThresholdTypes.Binary);


            Stopwatch sw = Stopwatch.StartNew();
            Random r = new Random();

            var maxErrors = d2.Cols * d2.Rows * (_pattern.PixelsMatchAcceptableErrorLevel / 100.0);

            int previewPixelsQty = (int)(10 + maxErrors);

            List<Point> points = new List<Point>();


            byte[,] screenMap = new byte[d.Cols, d.Rows];
            byte[,] patternMap = new byte[d2.Cols, d2.Rows];

            for (int i1 = 0; i1 < d.Cols; i1++)
            {
                for (int j1 = 0; j1 < d.Rows; j1++)
                {
                    var px2 = d.Get<byte>(j1, i1);
                    screenMap[i1, j1] = px2;

                }
            }
            for (int i1 = 0; i1 < d2.Cols; i1++)
            {
                for (int j1 = 0; j1 < d2.Rows; j1++)
                {
                    var px2 = d2.Get<byte>(j1, i1);
                    patternMap[i1, j1] = px2;

                }
            }
            for (int t = 0; t < previewPixelsQty; t++)
            {
                var rx = r.Next(d2.Width);
                var ry = r.Next(d2.Height);
                points.Add(new Point(rx, ry));

            }
            //slide window
            var www = d.Width - d2.Cols;
            var hhh = d.Height - d2.Rows;
            if (maxWidth != null)
                www = Math.Min(maxWidth.Value, www);

            if (maxHeight != null)
                hhh = Math.Min(maxHeight.Value, hhh);

            www -= d2.Width;
            hhh -= d2.Height;

            int errors = 0;
            for (int i = startX; i < www; i++)
            {
                for (int j = startY; j < hhh; j++)
                {
                    startY = 0;
                    bool good = true;
                    //pre check random pixels
                    for (int t = 0; t < points.Count; t++)
                    {
                        var rx = points[t].X;
                        var ry = points[t].Y;
                        var px = screenMap[i + rx, j + ry];
                        var px2 = patternMap[rx, ry];

                        if (px != px2)
                            errors++;

                        if (errors > maxErrors)
                        {
                            errors = 0;
                            good = false;
                            break;
                        }
                    }

                    if (!good)
                        continue;

                    //check pattern match
                    errors = 0;
                    for (int i1 = 0; i1 < d2.Cols; i1++)
                    {
                        for (int j1 = 0; j1 < d2.Rows; j1++)
                        {
                            var px = screenMap[i + i1, j + j1];
                            var px2 = patternMap[i1, j1];

                            if (px != px2)
                                errors++;

                            if (errors > maxErrors)
                            {
                                errors = 0;
                                good = false;
                                break;
                            }
                        }
                        if (!good)
                            break;
                    }

                    if (good)
                    {
                        sw.Stop();
                        return new System.Drawing.Point(i, j);
                    }
                }
            }

            return null;
        }

        private static Color ConvertPixel(PatternMatchingImageItem pattern, Color px)
        {
            switch (pattern.Mode)
            {
                case PatternMatchingMode.BinaryMean:
                    return ToGrayscale(px).R > 128 ? Color.White : Color.Black;

                case PatternMatchingMode.Grayscale:
                    return ToGrayscale(px);

                default:
                    return px;
            }
        }

        public override string ToXml()
        {            
            return $"<searchPattern patternId=\"{(Pattern == null ? string.Empty : Pattern.Id.ToString())}\" preCheck=\"{PreCheckCurrentPosition}\"" +
                $" clickIfSucceed=\"{ClickIfSucceed}\"" +
                $" mouseMoveIfSucceed=\"{MouseMoveIfSucceed}\"" +
                $" statusIfFailed=\"{StatusIfFailed}\"" +
                $" delayEnabled=\"{DelayEnabled}\"" +
                $" delay=\"{Delay}\"" +
                $" ></searchPattern>";
        }

        public bool Assert { get; set; }
    }
}