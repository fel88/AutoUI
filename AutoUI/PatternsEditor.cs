using AutoUI.TestItems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
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
            Pool.Patterns.Add(new PatternMatchingImage());
            updatePatternsList();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var tag = listView1.SelectedItems[0].Tag as PatternMatchingImage;
            updateSecondList(tag);

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
                listView2.Items.Add(new ListViewItem(new string[] {
                    t.Name,
                    t.Mode.ToString(),
                    t.PixelsMode.ToString(),
                    t.PixelsMatchDistancePerChannel.ToString()
                })
                { Tag = t });
            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
                return;

            var tag = listView2.SelectedItems[0].Tag as PatternMatchingImageItem;

            pictureBox1.Image = tag.Bitmap;
            toolStripStatusLabel1.Text = tag.Bitmap == null ? "null image" : $"Image: {tag.Bitmap.Width}x{tag.Bitmap.Height}";
        }

        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

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
            if (listView1.SelectedItems.Count == 0)
                return;

            if (listView1.SelectedItems.Count == 1)
            {
                var tag = (PatternMatchingImage)listView1.SelectedItems[0].Tag;
                if (!UIHelpers.ShowQuestion($"Are you sure you want to delete {tag.Name}?"))
                    return;
            }
            else if (!UIHelpers.ShowQuestion($"Are you sure to delete {listView1.SelectedItems.Count} elements?"))
                return;

            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                var tag = listView1.SelectedItems[i].Tag as PatternMatchingImage;
                Pool.Patterns.Remove(tag);
            }

            listView2.Items.Clear();
            updatePatternsList();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var tag = listView1.SelectedItems[0].Tag as PatternMatchingImage;
            if (listView2.SelectedItems.Count == 0)
                return;

            var tag2 = listView2.SelectedItems[0].Tag as PatternMatchingImageItem;
            tag.Items.Remove(tag2);
            updateSecondList(tag);

        }

        private void newFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(Clipboard.GetImage() is Bitmap bmp))
            {
                return;
            }

            PatternMatchingImage pp = new PatternMatchingImage();
            Pool.Patterns.Add(pp);

            pp.Items.Add(new PatternMatchingImageItem() { Bitmap = bmp });
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

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var tag = listView1.SelectedItems[0].Tag as PatternMatchingImage;

            var d = AutoDialog.DialogHelpers.StartDialog();
            d.AddStringField("name", "Name", tag.Name);

            if (!d.ShowDialog())
                return;

            tag.Name = d.GetStringField("name");

            updatePatternsList();
        }

        private void generatePaaternToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var tag = listView1.SelectedItems[0].Tag as PatternMatchingImage;
            if (listView2.SelectedItems.Count == 0)
                return;

            var tag2 = listView2.SelectedItems[0].Tag as PatternMatchingImageItem;

            var d = AutoDialog.DialogHelpers.StartDialog();
            d.AddStringField("name", "Name", tag2.Name);
            d.AddOptionsField("mode", "Mode", Enum.GetNames(typeof(PatternMatchingMode)), tag2.Mode.ToString());
            d.AddOptionsField("pmode", "Pixels mode", Enum.GetNames(typeof(PixelsMatchingMode)), tag2.PixelsMode.ToString());
            d.AddNumericField("pdist", "Pixels dist", tag2.PixelsMatchDistancePerChannel);

            if (!d.ShowDialog())
                return;

            tag2.Name = d.GetStringField("name");
            tag2.Mode = (PatternMatchingMode)Enum.Parse(typeof(PatternMatchingMode), d.GetOptionsField("mode"));
            tag2.PixelsMode = (PixelsMatchingMode)Enum.Parse(typeof(PixelsMatchingMode), d.GetOptionsField("pmode"));
            tag2.PixelsMatchDistancePerChannel = d.GetNumericField("pdist");

            updateSecondList(tag);
        }

        private void grabScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
            }

            PatternMatchingImage pp = new PatternMatchingImage();
            Pool.Patterns.Add(pp);

            CropImage crop = new CropImage();
            crop.Init(bitmap);
            if (crop.ShowDialog() == DialogResult.OK)
            {
                var temp = bitmap;
                bitmap = bitmap.Clone(crop.CropArea, PixelFormat.Format32bppArgb);
                temp.Dispose();
            }

            pp.Items.Add(new PatternMatchingImageItem() { Bitmap = bitmap });
            updatePatternsList();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
                return;

            var tag = listView2.SelectedItems[0].Tag as PatternMatchingImageItem;

            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            tag.Bitmap.Save(sfd.FileName, ImageFormat.Png);
        }
    }
}

