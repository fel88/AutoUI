using AutoDialog;
using AutoUI.TestItems;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AutoUI
{
    /*
  * todo: 
  * - test list
  * - run process
  * - waitPattern operator
  * - if operator and branching . better language to code.. maybe script language or graphical scenario editor. Dagre.NET should be use this case probably.
  * - states (set of condition)
  * - complex pattern (set of images which mean same pattern)
  * - loose pattern matching -> pre-binarize, percentrage equality allowed
  * - complex shapes searching and counting. rectangles for example
  * - debug TCP protocol. send code to execute on unicut side
  * - parametrized test with different input parameters (different files for example)
  * - keyboard input 
  * - text recognize from screen probably..
  * - mouse wheel events 
  * - ROI (seach only in regions of intereset) and count primitives there - number of horizontal lines or rectangles, etc.
  * - speed-up pattern search
  * 
  */
    public partial class mdi : Form
    {
        public mdi()
        {
            InitializeComponent();
        }

        public static TestSet set = new TestSet();
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            PatternsEditor f = new PatternsEditor();
            f.Init(set.Pool);            
            f.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<root>");
            set.AppendXml(sb);


            sb.AppendLine("</root>");
            sb.ToString();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Auto UI (axml files)|*.axml";
            if (sfd.ShowDialog() != DialogResult.OK) 
                return;

            File.WriteAllText(sfd.FileName, sb.ToString());
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Auto UI (axml files)|*.axml";

            if (ofd.ShowDialog() != DialogResult.OK) 
                return;

            set = new TestSet();
            var doc = XDocument.Load(ofd.FileName);
            var root = doc.Descendants("root").First();
            if (root.Element("pool") != null)
                set.Pool.ParseXml(set, root.Element("pool"));

            set.ParseXml(root);
            
            Init(set);            
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            AboutBox1 b = new AboutBox1();
            b.ShowDialog();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void fontMatcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontMatcher f = new FontMatcher();
            
            f.Show();
        }

        private void searchDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchDebug f = new SearchDebug();
            
            f.Show();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            set = new TestSet();
            
            foreach (var item in MdiChildren)
            {
                item.Close();
            }

            
            Init(set);
            
        }
        public void Init(TestSet _set)
        {
            set = _set;
            UpdateTestsList();
        }
        public void UpdateTestsList()
        {
            listView1.Items.Clear();
            foreach (var item in Set.Tests)
            {
                listView1.Items.Add(new ListViewItem(new string[] { item.Name, item.State.ToString(), "" }) { Tag = item });
            }
        }
        public TestSet Set => set;
        ListViewItem getLvi(object tag)
        {
            ListViewItem ret = null;
            listView1.Invoke((Action)(() =>
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].Tag == tag) { ret = listView1.Items[i]; break; }
                }
            }));

            return ret;
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
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

                    listView1.Invoke((Action)(() =>
                    {
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

                    }));
                   
                }
            });
            th.IsBackground = true;
            th.Start();


        }

        private void addTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Set.Tests.Add(new AutoTest(Set) { Name = "new test1" });
            UpdateTestsList();
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
        private void editSelected()
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var test = listView1.SelectedItems[0].Tag as AutoTest;
            Form1 f = new Form1();
            f.MdiParent = MdiParent;
            f.Init(test);
            f.Show();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editSelected();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateTestsList();
        }

        private void paramsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var test = listView1.SelectedItems[0].Tag as AutoTest;
            var d = DialogHelpers.StartDialog();

            d.AddStringField("name", "Name", test.Name);
            d.AddBoolField("emitter", "Use emitter", test.UseEmitter);
            d.AddEnumField("faction", "Failed action", test.FailedAction);

            if (!d.ShowDialog())
                return;

            test.Name = d.GetStringField("name");
            test.UseEmitter = d.GetBoolField("emitter");
            test.FailedAction = d.GetEnumField<TestFailedbehaviour>("faction");

            UpdateTestsList();
        }

        private void variablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var test = listView1.SelectedItems[0].Tag as AutoTest;
            VariablesEditor ve = new VariablesEditor();
            ve.Init(test);
            ve.ShowDialog();
        }

        private void exportReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //form for custom report?
            StringBuilder sb = new StringBuilder();
            int failed = 0;
            int total = 0;
            sb.AppendLine($"Param;Duration (sec);State");
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                var et = listView2.Items[i].Tag as EmittedSubTest;
                if (et.State == TestStateEnum.Failed) failed++;
                total++;
                sb.AppendLine($"{et.Data[et.Data.Keys.First()]};{et.Duration.TotalSeconds};{et.State}");
            }
            sb.AppendLine("");
            sb.AppendLine($"Total;{total};Failed;{failed}");
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() != DialogResult.OK) return;
            File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.Default);

            if (Helpers.Question("Open report?", Text))
            {
                Process.Start(sfd.FileName);
            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var et = listView2.SelectedItems[0].Tag as EmittedSubTest;
            if (et.lastContext != null && et.lastContext.WrongState != null)
            {
                toolStripStatusLabel1.Text = $"wrong state: {et.lastContext.WrongState.Id}  {et.lastContext.WrongState.GetType().Name}";
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var test = listView1.SelectedItems[0].Tag as AutoTest;

            if (test.lastContext != null && test.lastContext.WrongState != null)
            {
                toolStripStatusLabel1.Text = $"wrong state: {test.lastContext.WrongState.Id}  {test.lastContext.WrongState.GetType().Name}";
            }
            if (test.lastContext != null)
                updateSubTestList(test.lastContext);

        }
        private void updateSubTestList(AutoTestRunContext lastContext)
        {
            listView2.Items.Clear();
            foreach (var item in lastContext.SubTests)
            {
                var lvi = new ListViewItem(new string[] { "sub", item.State.ToString(), item.Duration.TotalSeconds + "s", item.FinishTime.ToLongTimeString() }) { Tag = item };
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

    }
}
