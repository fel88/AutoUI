using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AutoUI
{
    public class ConnectedComponent
    {
        public void Crop()
        {
            var bbox = BoundingBox();
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] = new Point(Points[i].X - bbox.X, Points[i].Y - bbox.Y);
            }
        }

        public Rectangle BoundingBox()
        {
            var minx = Points.Min(z => z.X);
            var miny = Points.Min(z => z.Y);
            var maxx = Points.Max(z => z.X);
            var maxy = Points.Max(z => z.Y);
            return new Rectangle(minx, miny, maxx - minx + 1, maxy - miny + 1);
        }

        public Bitmap ToBitmap()
        {
            var bb = BoundingBox();
            Bitmap bmp = new Bitmap(bb.Width, bb.Height);
            var gr = Graphics.FromImage(bmp);
            gr.Clear(Color.White);
            foreach (var item in Points)
            {
                bmp.SetPixel(item.X - bb.Left, item.Y - bb.Top, Color.Black);
            }

            return bmp;
        }

        public bool IsEqual(ConnectedComponent cc)
        {
            if (Points.Count != cc.Points.Count) return false;
            HashSet<string> ss = new HashSet<string>();
            foreach (var item in Points)
            {
                ss.Add(item.X + ";" + item.Y);
            }
            if (ss.Count != Points.Count)
            {
                //error. count mismatch
            }
            foreach (var item in cc.Points)
            {
                if (ss.Add(item.X + ";" + item.Y))
                {
                    return false;
                }
            }
            return true;
        }

        public List<Point> Points = new List<Point>();
    }
}

