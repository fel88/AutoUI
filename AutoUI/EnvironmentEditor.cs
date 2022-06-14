using AutoUI.TestItems;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
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
            Thread th = new Thread(() =>
            {
            foreach (var item in Set.Tests)
            {
                var res = item.Run();
                    int cc = 0;
                    foreach (var sub in res.SubTests)
                    {
                        toolStripStatusLabel3.Visible = true;
                        toolStripStatusLabel3.Text = "subtests: " + cc + " / " + res.SubTests.Count;
                        var res1 = sub.Run();
                        cc++;
                    }
                    toolStripStatusLabel3.Visible = false;
                    if (item.UseEmitter)
                        item.State = TestStateEnum.Emitter;

                var lvi = getLvi(item);
                    if (item.State == TestStateEnum.Failed)
                    {
                        lvi.BackColor = Color.Red;
                        lvi.ForeColor = Color.White;
                    }
                    if (item.State == TestStateEnum.Success)
                    {
                        lvi.BackColor = Color.LightGreen;
                        lvi.ForeColor = Color.Black;
            }
                    if (item.State == TestStateEnum.Emitter)
                    {
                        lvi.BackColor = Color.Violet;
                        lvi.ForeColor = Color.White;
                    }
                    lvi.SubItems[1].Text = item.State.ToString();
                    lvi.SubItems[2].Text = DateTime.Now.ToLongTimeString();
                }
            });
            th.IsBackground = true;
            th.Start();


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
            var test = listView1.SelectedItems[0].Tag as AutoTest;
            propertyGrid1.SelectedObject = listView1.SelectedItems[0].Tag;
            if (test.lastContext != null && test.lastContext.WrongState != null)
            {
                label1.Text = "wrong state: " + test.lastContext.WrongState.Id + "  " + test.lastContext.WrongState.GetType().Name;
            }
            if (test.lastContext != null)
                updateSubTestList(test.lastContext);
        }

        private void updateSubTestList(AutoTestRunContext lastContext)
        {
            listView2.Items.Clear();
            foreach (var item in lastContext.SubTests)
            {
                var lvi = new ListViewItem(new string[] { "sub", item.State.ToString(), item.FinishTime.ToLongTimeString() }) { Tag = item };
                listView2.Items.Add(lvi);

                if (item.State == TestStateEnum.Failed)
                {
                    lvi.BackColor = Color.Red;
                    lvi.ForeColor = Color.White;
                }
                if (item.State == TestStateEnum.Success)
                {
                    lvi.BackColor = Color.LightGreen;
                    lvi.ForeColor = Color.Black;
                }
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateTestsList();
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0) return;
            var et = listView2.SelectedItems[0].Tag as EmittedSubTest;
            if (et.lastContext != null && et.lastContext.WrongState != null)
            {
                label1.Text = "wrong state: " + et.lastContext.WrongState.Id + "  " + et.lastContext.WrongState.GetType().Name;
    }
            listView3.Items.Clear();
            foreach (var item in et.Data)
    {
                listView3.Items.Add(new ListViewItem(new string[] { item.Key, item.Value.ToString() }) { Tag = item });
            }
        }
    }
}
