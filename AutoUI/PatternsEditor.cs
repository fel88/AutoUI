using AutoUI.TestItems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUI
{
    public partial class PatternsEditor : Form
    {
        public PatternsEditor()
        {
            InitializeComponent();
            //todo: patterns with synonyms as strict image
            // functional loose pattern matching
            pictureBox1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        }
        PatternMatchingPool Pool;
        public void Init(PatternMatchingPool pool)
        {
            Pool = pool;
            updatePatternsList();
        }

        public void updatePatternsList()
        {
            listView1.Items.Clear();
            foreach (var item in Pool.Patterns)
            {
                listView1.Items.Add(new ListViewItem(new string[] { item.Name, "" }) { Tag = item });
            }
        }

        private void addPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pool.Patterns.Add(new TestItems.PatternMatchingImage());
            updatePatternsList();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var tag = listView1.SelectedItems[0].Tag as PatternMatchingImage;
            updateSecondList(tag);
            propertyGrid1.SelectedObject = tag;
            if (tag.Items.Any() && tag.Items.First().Bitmap != null)
                pictureBox1.Image = tag.Items.First().Bitmap;
            else
                pictureBox1.Image = null;
        }

        private void updateSecondList(PatternMatchingImage tag)
        {
            listView2.Items.Clear();
            foreach (var t in tag.Items)
            {
                listView2.Items.Add(new ListViewItem(new string[] { t.Name }) { Tag = t });
            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0) return;
            var tag = listView2.SelectedItems[0].Tag as PatternMatchingImageItem;
            propertyGrid1.SelectedObject = tag;
            pictureBox1.Image = tag.Bitmap;
        }

        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK) return;
            var tag = listView1.SelectedItems[0].Tag as PatternMatchingImage;
            tag.Items.Add(new PatternMatchingImageItem() { Bitmap = Bitmap.FromFile(ofd.FileName) as Bitmap });
            updateSecondList(tag);
        }

        private void fromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;


            var tag = listView1.SelectedItems[0].Tag as PatternMatchingImage;
            tag.Items.Add(new PatternMatchingImageItem() { Bitmap = Clipboard.GetImage() as Bitmap });
            updateSecondList(tag);
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updatePatternsList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var tag = listView1.SelectedItems[0].Tag as PatternMatchingImage;
            Pool.Patterns.Remove(tag);
            updatePatternsList();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var tag = listView1.SelectedItems[0].Tag as PatternMatchingImage;
            if (listView2.SelectedItems.Count == 0) return;
            var tag2 = listView2.SelectedItems[0].Tag as PatternMatchingImageItem;
            tag.Items.Remove(tag2);
            updateSecondList(tag);

        }

        private void newFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatternMatchingImage pp = new PatternMatchingImage();
            Pool.Patterns.Add(pp);

            pp.Items.Add(new PatternMatchingImageItem() { Bitmap = Clipboard.GetImage() as Bitmap });
            updatePatternsList();
        }

        private void fromFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ///todo: generate automatic pattern from specific font
            ///
            FontDialog fd = new FontDialog();
            fd.ShowDialog();

            var pmi = new PatternMatchingImage() { Name = "generatedFont" };
            Pool.Patterns.Add(pmi);

            var font = fd.Font;

            Bitmap bmp = new Bitmap(20, 20);
            var gr = Graphics.FromImage(bmp);
            List<char> chars = new List<char>();

            for (char i = '0'; i <= '9'; i++)
            {
                chars.Add(i);
            }
            for (char i = 'a'; i <= 'z'; i++)
            {
                chars.Add(i);
            }
            for (char i = 'A'; i <= 'Z'; i++)
            {
                chars.Add(i);
            }
            for (char i = 'а'; i <= 'я'; i++)
            {
                chars.Add(i);
            }
            for (char i = 'А'; i <= 'Я'; i++)
            {
                chars.Add(i);
            }
            /*chars.Add('.');
            chars.Add(',');
            chars.Add(';');
            chars.Add('-');
            chars.Add(':');
            chars.Add('!');*/
            foreach (var item in chars)
            {
                gr.Clear(Color.White);
                gr.DrawString(item + "", font, Brushes.Black, 0, 0);

                var img = FontMatcher.CropBorder(bmp) as Bitmap;
                var pp = (new PatternMatchingImageItem() { Name = "char:" + item, Mode = PatternMatchingMode.BinaryMean, Bitmap = img });
                pmi.Items.Add(pp);
            }


            updatePatternsList();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void fromScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var img = Clipboard.GetImage() as Bitmap;
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    var px = SearchByPatternImage.ToBinaryMean(img.GetPixel(i, j));
                    img.SetPixel(i, j, px);
                }
            }
            Clipboard.SetImage(img);
            //find all black connected components
            var cc = GetConnectedComponents(img);

            //skip same
            foreach (var item in cc)
            {
                item.Crop();
            }
            List<ConnectedComponent> toDel = new List<ConnectedComponent>();
            for (int i = 0; i < cc.Length; i++)
            {
                for (int j = i + 1; j < cc.Length; j++)
                {
                    if (cc[i].IsEqual(cc[j]))
                    {
                        toDel.Add(cc[j]);
                    }
                }
            }
            cc = cc.Except(toDel).ToArray();
      


            var pmi = new PatternMatchingImage() { Name = "generatedFont" };
            Pool.Patterns.Add(pmi);

            foreach (var item in cc)
            {
                //get bitmap and crop
                var bb = item.ToBitmap();
                pmi.Items.Add(new PatternMatchingImageItem() { Bitmap = bb, Mode = PatternMatchingMode.BinaryMean });
            }

            updatePatternsList();
        }

        public static ConnectedComponent[] GetConnectedComponents(Bitmap bmp)
        {
            List<ConnectedComponent> components = new List<ConnectedComponent>();
            List<Point> pps = new List<Point>();
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    var px = bmp.GetPixel(i, j);
                    if (px.R < 128)
                    {
                        pps.Add(new Point(i, j));
                    }
                }
            }

            var remains = pps.ToList();
            while (remains.Any())
            {
                ConnectedComponent cc = new ConnectedComponent();
                Queue<Point> q = new Queue<Point>();
                q.Enqueue(remains[0]);

                while (q.Count > 0)
                {
                    var deq = q.Dequeue();
                    remains.Remove(deq);
                    cc.Points.Add(deq);
                    foreach (var item in remains)
                    {
                        var dx = Math.Abs(item.X - deq.X);
                        var dy = Math.Abs(item.Y - deq.Y);
                        if (dx <= 1 && dy <= 1)
                            q.Enqueue(item);
                    }
                }
                components.Add(cc);
            }

            return components.ToArray();
        }
    }

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

