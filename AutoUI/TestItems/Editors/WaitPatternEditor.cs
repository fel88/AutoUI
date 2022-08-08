using System;
using System.Linq;
using System.Windows.Forms;

namespace AutoUI.TestItems.Editors
{
    public partial class WaitPatternEditor : UserControl, ITestItemEditor
    {
        public WaitPatternEditor()
        {
            InitializeComponent();
        }
        public WaitPatternImage TestItem;

        public void Init(AutoTestItem item)
        {
            TestItem = item as WaitPatternImage;
            UpdateList();
        }

        public void UpdateList()
        {
            listView1.Items.Clear();
            foreach (var pattern in TestItem.Patterns)
            {
                listView1.Items.Add(new ListViewItem(new string[] { pattern.Name }) { Tag = pattern });
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var p = listView1.SelectedItems[0].Tag as PatternMatchingImage;
            pictureBox1.Image = p.Items.First().Bitmap;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void addPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatternSelector s = new PatternSelector();
            s.Init(TestItem.ParentTest.Parent.Pool);
            if (s.ShowDialog() != DialogResult.OK) return;
            TestItem.Patterns.Add(s.Selected);
            UpdateList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var p = listView1.SelectedItems[0].Tag as PatternMatchingImage;
            TestItem.Patterns.Remove(p);
            UpdateList();
        }
    }
}
