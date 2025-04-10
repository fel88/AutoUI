using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AutoUI.TestItems.ScreenshotTestItem;
using static System.Windows.Forms.AxHost;

namespace AutoUI
{
    public partial class CropImage : Form
    {
        public CropImage()
        {
            InitializeComponent();
            ctx.Init(pictureBoxWithInterpolationMode1);
            pictureBoxWithInterpolationMode1.Paint += PictureBoxWithInterpolationMode1_Paint;
            pictureBoxWithInterpolationMode1.MouseUp += PictureBox1_MouseUp;
            pictureBoxWithInterpolationMode1.MouseDown += PictureBox1_MouseDown;
            pictureBoxWithInterpolationMode1.MouseWheel += PictureBox1_MouseWheel;
        }
        public float sx, sy;
        public float rsx, rsy;
        public float zoom = 1;
        public PointF Transform(PointF p1)
        {
            return new PointF((p1.X + sx) * zoom, -1 * (p1.Y + sy) * zoom);
        }
        public PointF Transform(Point p1)
        {
            return new PointF((p1.X + sx) * zoom, -1 * (p1.Y + sy) * zoom);
        }
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            var pos = pictureBoxWithInterpolationMode1.PointToClient(Cursor.Position);
            var p = Transform(pos);

            if (e.Button == MouseButtons.Right)
            {
                drag = true;
                startx = pos.X;
                starty = pos.Y;
                origsx = sx;
                origsy = sy;
            }
            if (e.Button == MouseButtons.Left)
            {
                rectSelection = true;
                rect1 = BackTransform(pos);
                rstartx = pos.X;
                rstarty = pos.Y;
                rorigsx = sx;
                rorigsy = sy;
            }
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
            rectSelection = false;

            var p = pictureBoxWithInterpolationMode1.PointToClient(Cursor.Position);
            var pos = pictureBoxWithInterpolationMode1.PointToClient(Cursor.Position);
            var posx = (pos.X / zoom - sx);
            var posy = (-pos.Y / zoom - sy);
        }

        bool drag = false;
        bool rectSelection = false;
        float startx, starty;
        float origsx, origsy;
        float rstartx, rstarty;
        float rorigsx, rorigsy;
        PointF rect1;
        PointF rect2;

        public virtual PointF BackTransform(PointF p1)
        {
            return new PointF((p1.X / zoom - sx), -(p1.Y / zoom + sy));
        }
        private void PictureBoxWithInterpolationMode1_Paint(object sender, PaintEventArgs e)
        {
            var pos = pictureBoxWithInterpolationMode1.PointToClient(Cursor.Position);


            if (drag)
            {


                sx = origsx + ((pos.X - startx) / zoom);
                sy = origsy + (-(pos.Y - starty) / zoom);
            }
            if (rectSelection)
            {
                rect2 = BackTransform(pos);


            }
            var gr = e.Graphics;
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            gr.Clear(Color.White);
            var p0 = Transform(new PointF(0, 0));
            var p1 = Transform(new PointF(source.Width, source.Height));
            var p2 = Transform(new PointF(100, 0));
            var p3 = Transform(new PointF(0, 100));
            gr.DrawLine(Pens.Red, p0, p2);
            gr.DrawLine(Pens.Blue, p0, p3);
            var r1 = Transform(rect1);
            var r2 = Transform(rect2);


            gr.DrawImage(source, new RectangleF(p0.X, p0.Y, p1.X - p0.X, -(p1.Y - p0.Y)),
                new RectangleF(0, 0, source.Width, source.Height), GraphicsUnit.Pixel);

            var p = new Pen(Color.Aqua, 2);
            p.DashPattern = new float[] { 2, 5 };
            gr.DrawRectangle(p, r1.X, r1.Y, r2.X - r1.X, r2.Y - r1.Y);

        }

        DrawingContext ctx = new DrawingContext();
        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {

            float zold = zoom;
            if (e.Delta > 0) { zoom *= 1.5f; ; }
            else { zoom *= 0.5f; }
            if (zoom < 0.08) { zoom = 0.08f; }
            if (zoom > 1000) { zoom = 1000f; }

            var pos = pictureBoxWithInterpolationMode1.PointToClient(Cursor.Position);

            sx = -(pos.X / zold - sx - pos.X / zoom);
            sy = (pos.Y / zold + sy - pos.Y / zoom);
        }
        public RectangleF CropArea { get; set; } = new Rectangle();
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            CropArea = new RectangleF(Math.Min(rect1.X, rect2.X), Math.Min(-rect1.Y, -rect2.Y), Math.Abs(rect2.X - rect1.X), Math.Abs(rect2.Y - rect1.Y));
            DialogResult = DialogResult.OK;
            Close();
        }
        Bitmap source;
        internal void Init(Bitmap bitmap)
        {
            source = bitmap;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBoxWithInterpolationMode1.Invalidate();
        }
    }
}
