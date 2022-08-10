using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

using System.Windows.Forms;

namespace AutoUI
{
    public partial class FontMatcher : Form
    {
        public FontMatcher()
        {
            InitializeComponent();

            bmp1 = new Bitmap(250, 250);
            bmp2 = new Bitmap(250, 250);
            gr1 = Graphics.FromImage(bmp1);
            gr2 = Graphics.FromImage(bmp2);
        }



        Graphics gr1;
        Graphics gr2;

        private void fromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Clipboard.GetImage();
            pictureBoxWithInterpolationMode1.Image = Clipboard.GetImage();
        }

        Font font = new Font("Tahoma", 8);
        private void button1_Click(object sender, EventArgs e)
        {
            var img = pictureBox1.Image;
            Bitmap bmp = new Bitmap(img.Width * 2, img.Height * 2);
            var gr = Graphics.FromImage(bmp);
            //var fm = FontFamily.GetFamilies(gr);
            List<Font> fonts = new List<Font>();
            fonts.Add(font);

            //fonts.Add(new Font("Verdana", 10));
            //fonts.Add(new Font("Arial", 10));
            foreach (var item in fonts)
            {
                gr.Clear(Color.White);
                gr.DrawString(textBox1.Text, item, Brushes.Black, 0, 0);

                pictureBoxWithInterpolationMode2.Image = CropBorder(bmp);
            }
        }

        public static Image CropBorder(Bitmap bmp)
        {
            var borderColor = bmp.GetPixel(0, 0);
            List<Point> pp = new List<Point>();
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    if (bmp.GetPixel(i, j) != borderColor)
                        pp.Add(new Point(i, j));
                }
            }

            var minx = pp.Min(z => z.X);
            var miny = pp.Min(z => z.Y);
            var maxx = pp.Max(z => z.X);
            var maxy = pp.Max(z => z.Y);           

            var w = maxx - minx + 1;
            var h = maxy - miny + 1;            

            Bitmap ret = new Bitmap(w, h);
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    ret.SetPixel(i, j, bmp.GetPixel(i + minx, j + miny));
                }
            }
            return ret;
        }

        bool pixelEq(Color c1, Color c2)
        {
            return (c1.ToArgb() & 0xFFFFFF) == (c2.ToArgb() & 0xFFFFFF);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var im1 = pictureBoxWithInterpolationMode1.Image as Bitmap;
            var im2 = pictureBoxWithInterpolationMode2.Image as Bitmap;
            int match = 0;
            var minw = Math.Min(im1.Width, im2.Width);
            var minh = Math.Min(im1.Height, im2.Height);
            for (int i = 0; i < minw; i++)
            {
                for (int j = 0; j < minh; j++)
                {
                    var px = im1.GetPixel(i, j);
                    var px2 = im2.GetPixel(i, j);
                    if (pixelEq(px, px2))
                    {
                        match++;
                    }
                }
            }

            var total = minw * minh;
            var perc = match / (float)total;
            toolStripStatusLabel1.Text = "match: " + match + "; " + minw * minh + ": perc: " + (perc * 100) + "%";
        }

        private void pictureBoxWithInterpolationMode1_Click(object sender, EventArgs e)
        {
            pictureBoxWithInterpolationMode1.Image.Save("temp1.png");
            Process.Start("temp1.png");
        }

        private void pictureBoxWithInterpolationMode2_Click(object sender, EventArgs e)
        {
            pictureBoxWithInterpolationMode2.Image.Save("temp2.png");
            Process.Start("temp2.png");
        }
        int cellw = 15;

        Bitmap bmp1;
        Bitmap bmp2;
        private void timer1_Tick(object sender, EventArgs e)
        {
            gr1.Clear(Color.White);
            gr2.Clear(Color.White);

            var img1 = pictureBoxWithInterpolationMode1.Image as Bitmap;
            if (img1 == null) return;
            var img2 = pictureBoxWithInterpolationMode2.Image as Bitmap;
            if (img2 == null) return;

            var curp = pictureBox1.PointToClient(Cursor.Position);
            var cx = curp.X / cellw;
            var cy = curp.Y / cellw;


            for (int i = 0; i < img1.Width; i++)
            {
                for (int j = 0; j < img1.Height; j++)
                {
                    var px1 = img1.GetPixel(i, j);
                    gr1.FillRectangle(new SolidBrush(px1), i * cellw, j * cellw, cellw, cellw);
                }
            }
            if (cx >= 0 && cy >= 0)
            {
                try
                {
                    var px = img1.GetPixel(cx, cy);
                    gr1.DrawRectangle(Pens.Red, cx * cellw, cy * cellw, cellw, cellw);
                    toolStripStatusLabel1.Text = "hovered: " + cx + "x" + cy + ": " + px;
                }
                catch (Exception ex)
                {

                }
            }

            curp = pictureBox2.PointToClient(Cursor.Position);
            cx = curp.X / cellw;
            cy = curp.Y / cellw;
            for (int i = 0; i < img2.Width; i++)
            {
                for (int j = 0; j < img2.Height; j++)
                {
                    var px1 = img2.GetPixel(i, j);
                    gr2.FillRectangle(new SolidBrush(px1), i * cellw, j * cellw, cellw, cellw);
                }
            }
            if (cx >= 0 && cy >= 0)
            {
                try
                {
                    var px = img2.GetPixel(cx, cy);
                    gr2.DrawRectangle(Pens.Red, cx * cellw, cy * cellw, cellw, cellw);
                    toolStripStatusLabel1.Text = "hovered: " + cx + "x" + cy + ": " + px;
                }
                catch (Exception ex)
                {

                }
            }

            pictureBox1.Image = bmp1;
            pictureBox2.Image = bmp2;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            cellw = (int)numericUpDown1.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var img1 = pictureBoxWithInterpolationMode1.Image;
            pictureBoxWithInterpolationMode1.Image = threshold(img1 as Bitmap);
            pictureBoxWithInterpolationMode2.Image = threshold(pictureBoxWithInterpolationMode2.Image as Bitmap);
        }

        private Image threshold(Bitmap img1, int eps = 128)
        {
            Bitmap ret = new Bitmap(img1.Width, img1.Height);
            for (int i = 0; i < img1.Width; i++)
            {
                for (int j = 0; j < img1.Height; j++)
                {
                    var px = img1.GetPixel(i, j);
                    var mean = (px.R + px.G + px.B) / 3;
                    if (mean > eps)
                    {
                        ret.SetPixel(i, j, Color.White);
                    }
                    else
                    {
                        ret.SetPixel(i, j, Color.Black);
                    }
                }
            }
            //cut by white?
            return ret;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = font;
            fd.ShowDialog();
            font = fd.Font;
            toolStripStatusLabel1.Text = font.ToString();
        }
    }
}
