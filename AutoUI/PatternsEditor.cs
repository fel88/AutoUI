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
            var tag = listView2.SelectedItems[0].Tag as PatternMatchingImage.PatternMatchingImageItem;
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
            tag.Items.Add(new PatternMatchingImage.PatternMatchingImageItem() { Bitmap = Bitmap.FromFile(ofd.FileName) as Bitmap });
            updateSecondList(tag);
        }

        private void fromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;


            var tag = listView1.SelectedItems[0].Tag as PatternMatchingImage;
            tag.Items.Add(new PatternMatchingImage.PatternMatchingImageItem() { Bitmap = Clipboard.GetImage() as Bitmap });
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
            var tag2 = listView2.SelectedItems[0].Tag as PatternMatchingImage.PatternMatchingImageItem;
            tag.Items.Remove(tag2);
            updateSecondList(tag);

        }

        private void newFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatternMatchingImage pp = new PatternMatchingImage();
            Pool.Patterns.Add(pp);
            
            pp.Items.Add(new PatternMatchingImage.PatternMatchingImageItem() { Bitmap = Clipboard.GetImage() as Bitmap });
            updatePatternsList();
        }
    }
}
