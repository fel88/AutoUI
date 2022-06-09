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
    public partial class PatternSelector : Form
    {
        public PatternSelector()
        {
            InitializeComponent();
        }
        public PatternMatchingImage Selected;
        public void Init(PatternMatchingPool pool)
        {
            listView1.Items.Clear();
            foreach (var item in pool.Patterns)
            {
                listView1.Items.Add(new ListViewItem(new string[] { item.Name }) { Tag = item });
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var p = listView1.SelectedItems[0].Tag as PatternMatchingImage;
            pictureBox1.Image = p.Items.First().Bitmap;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var p = listView1.SelectedItems[0].Tag as PatternMatchingImage;
            Selected = p;
            Close();
        }
    }
}
