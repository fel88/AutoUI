using AutoUI.Common;
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
            {
                var k = pictureBox1.Width / (float)bmp.Width;
                e.Graphics.ResetTransform();
                e.Graphics.ScaleTransform(k, k);
                e.Graphics.DrawImage(bmp, 0, 0);
            }
            foreach (var item in infos)
            {
                e.Graphics.DrawRectangle(Pens.Red, item.Rect);
            }


        }
        List<PatternFindInfo> infos = new List<PatternFindInfo>();

        Bitmap bmp;


        private void button1_Click(object sender, EventArgs e)
        {
            if (bmp == null)
            {
                Helpers.Error("bmp is null");
                return;
            }
            if (pattern == null)
            {
                Helpers.Error("pattern is null");
                return;
            }
            infos.Clear();
            var sw = Stopwatch.StartNew();

            foreach (var item in pattern.Items)
            {
                if (item.Mode == PatternMatchingMode.TemplateMatching)
                {
                    var results = SearchByPatternImage.TemplateMatchingAll(bmp, item);

                    foreach (var res in results)
                    {
                        infos.Add(new PatternFindInfo()
                        {
                            Pattern = item,
                            Rect = new Rectangle(res.Rect.X, res.Rect.Y, item.Bitmap.Width, item.Bitmap.Height)
                        });
                    }

                }
                else
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
            ps.Init(Tests.set.Pool);
            ps.ShowDialog();
            pattern = ps.Selected;
            label1.Text = $"pattern: {pattern.Name}";
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

        private void grabScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
            }
            bmp = bitmap;


        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp1 = new Bitmap(bmp.Width, bmp.Height);
            using (var gr = Graphics.FromImage(bmp1))
            {
                gr.DrawImageUnscaled(bmp, 0, 0);
                //pictureBox1.DrawToBitmap(bmp, bmp1.GetBounds();
                foreach (var item in infos)
                {
                    gr.DrawRectangle(Pens.Red, item.Rect);
                }
                bmp1.Save("temp1.png");
                Process.Start("temp1.png");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (bmp == null)
            {
                Helpers.Error("bmp is null");
                return;
            }
            if (pattern == null)
            {
                Helpers.Error("pattern is null");
                return;
            }
            infos.Clear();
            var sw = Stopwatch.StartNew();
            foreach (var item in pattern.Items)
            {
                Point? res = SearchByPatternImage.SearchPattern(bmp, item, 0, 0);
                if (res == null)
                    continue;

                infos.Add(new PatternFindInfo()
                {
                    Pattern = item,
                    Rect = new Rectangle(res.Value.X, res.Value.Y, item.Bitmap.Width, item.Bitmap.Height)
                });
                break;
            }

            //non-max suppress

            sw.Stop();
            toolStripStatusLabel1.Text = $"Complete. Founded: {infos.Count}; time: {sw.ElapsedMilliseconds}ms";

            listView1.Items.Clear();
            foreach (var item in infos)
            {
                listView1.Items.Add(new ListViewItem(new string[] { item.Rect.ToString() }) { Tag = item });
            }
        }
    }
}
