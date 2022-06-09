using AutoUI.TestItems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUI
{
    public partial class EnvironmentEditor : Form
    {
        public EnvironmentEditor()
        {
            InitializeComponent();
        }


        public void Init(TestSet set)
        {
            Set = set;
            UpdateTestsList();
        }
        public TestSet Set;

        public void UpdateTestsList()
        {
            listView1.Items.Clear();
            foreach (var item in Set.Tests)
            {
                listView1.Items.Add(new ListViewItem(new string[] { item.Name, item.State.ToString(), "" }) { Tag = item });
            }
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            editSelected();
        }

        private void editSelected()
        {
            if (listView1.SelectedItems.Count == 0) return;

            var test = listView1.SelectedItems[0].Tag as AutoTest;
            Form1 f = new Form1();
            f.MdiParent = MdiParent;
            f.Init(test);
            f.Show();
        }

        private void addTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Set.Tests.Add(new AutoTest(Set) { Name = "new test1" });
            UpdateTestsList();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        ListViewItem getLvi(object tag)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Tag == tag) return listView1.Items[i];
            }
            return null;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (var item in Set.Tests)
            {
                var res = item.Run();

                var lvi = getLvi(item);
                lvi.SubItems[2].Text = item.State.ToString();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            if (MessageBox.Show("sure to del?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var test = listView1.SelectedItems[0].Tag as AutoTest;
                Set.Tests.Remove(test);
                UpdateTestsList();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editSelected();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            propertyGrid1.SelectedObject = listView1.SelectedItems[0].Tag;
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateTestsList();
        }
    }

    public class TestSet
    {
        public string Name { get; set; }
        public string ProcessPath;
        public List<AutoTest> Tests = new List<AutoTest>();
        public PatternMatchingPool Pool = new PatternMatchingPool();
    }
}
