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
    public partial class EnvironmentEditor : Form
    {
        public EnvironmentEditor()
        {
            InitializeComponent();
        }

        public TestSet Set = new TestSet();

        public void UpdateTestsList()
        {
            listView1.Items.Clear();
            foreach (var item in Set.Tests)
            {
                listView1.Items.Add(new ListViewItem(new string[] { item.Name, item.State.ToString()}) { Tag = item });
            }
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
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
            Set.Tests.Add(new AutoTest() { Name = "new test1" });
            UpdateTestsList();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }
    }

    public class TestSet
    {
        public string Name { get; set; }
        public string ProcessPath;
        public List<AutoTest> Tests = new List<AutoTest>();
    }
}
