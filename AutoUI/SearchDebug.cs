using AutoUI.TestItems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AutoUI
{
    public partial class SearchDebug : Form
    {
        public SearchDebug()
        {
            InitializeComponent();
            pictureBox3.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            pictureBox2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            pictureBox1.Paint += PictureBox1_Paint;
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            if (bmp != null)
                e.Graphics.DrawImage(bmp, 0, 0);
            foreach (var item in infos)
            {
                e.Graphics.DrawRectangle(Pens.Red, item.Rect);
            }


        }
        List<PatternFindInfo> infos = new List<PatternFindInfo>();

        Bitmap bmp;


        private void button1_Click(object sender, EventArgs e)
        {
            infos.Clear();
            var sw = Stopwatch.StartNew();
            foreach (var item in pattern.Items)
            {
                Point? res = null;
                do
                {
                    res = SearchByPatternImage.SearchPattern(bmp, item, res == null ? 0 : (res.Value.X), res == null ? 0 : (res.Value.Y + 1));
                    if (res != null)
                        infos.Add(new PatternFindInfo()
                        {
                            Pattern = item,
                            Rect = new Rectangle(res.Value.X, res.Value.Y, item.Bitmap.Width, item.Bitmap.Height)
                        });

                } while (res != null);
            }

            //non-max suppress

            var ord = infos.OrderByDescending(z => z.Rect.Width * z.Rect.Height).ToArray();
            List<PatternFindInfo> todel = new List<PatternFindInfo>();
            for (int i = 0; i < ord.Length; i++)
            {
                for (int j = i + 1; j < ord.Length; j++)
                {
                    if (ord[i].Rect.Contains(ord[j].Rect))
                    {
                        todel.Add(ord[j]);
                    }
                }
            }

            foreach (var item in todel)
            {
                infos.Remove(item);
            }

            infos = infos.OrderBy(z => z.Rect.Left).ThenBy(z => z.Rect.Top).ToList();
            sw.Stop();
            toolStripStatusLabel1.Text = "Complete. Founded: " + infos.Count + "; time: " + sw.ElapsedMilliseconds + "ms";

            listView1.Items.Clear();
            foreach (var item in infos)
            {
                listView1.Items.Add(new ListViewItem(new string[] { item.Rect.ToString() }) { Tag = item });
            }
        }


        PatternMatchingImage pattern;
        private void button2_Click(object sender, EventArgs e)
        {
            PatternSelector ps = new PatternSelector();
            ps.Init(mdi.set.Pool);
            ps.ShowDialog();
            pattern = ps.Selected;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void setFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bmp = Clipboard.GetImage() as Bitmap;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bmp, pictureBox1.ClientRectangle);
            bmp.Save("temp1.png");
            Process.Start("temp1.png");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var t = listView1.SelectedItems[0].Tag as PatternFindInfo;
            Bitmap bb = new Bitmap(t.Pattern.Bitmap.Width, t.Pattern.Bitmap.Height);
            var gr = Graphics.FromImage(bb);
            gr.DrawImage(bmp, -t.Rect.X, -t.Rect.Y);
            gr.Dispose();

            pictureBox2.Image = bb;
            pictureBox3.Image = t.Pattern.Bitmap;
        }
    }
    public class PatternFindInfo
    {
        public PatternMatchingImageItem Pattern;
        public Rectangle Rect;
    }
}
