using AutoDialog;
using AutoUI.CodeEditor;
using AutoUI.Common;
using AutoUI.Editors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AutoUI
{
    /*
  * todo: 
  * - states (set of condition)
  * - complex shapes searching and counting. rectangles for example
  * - text recognize from screen probably..  
  * - ROI (seach only in regions of intereset) and count primitives there - number of horizontal lines or rectangles, etc.  
  * 
  */
    public partial class Tests : Form
    {
        public Tests()
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
            f.MdiParent = MdiParent;
            f.Init(set.Pool);
            f.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Auto UI zip (azip files)|*.azip|Auto UI (axml files)|*.axml";
            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            if (sfd.FileName.ToLower().EndsWith(".azip"))
            {
                using (var compressedFileStream = new MemoryStream())
                {
                    set.ToStream(compressedFileStream);
                    File.WriteAllBytes(sfd.FileName, compressedFileStream.ToArray());
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<?xml version=\"1.0\"?>");
                sb.AppendLine("<root>");
                var el1 = set.ToXml();
                sb.AppendLine(el1.ToString());

                sb.AppendLine("</root>");
                sb.ToString();


                var doc = XDocument.Parse(sb.ToString());
                doc.Save(sfd.FileName);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Auto UI (axml, azip files)|*.axml;*.azip";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            if (ofd.FileName.ToLower().EndsWith(".axml"))
            {
                var doc = XDocument.Load(ofd.FileName);
                set = new TestSet(doc.Root.Element("set"));
            }
            else
                set = TestSet.LoadFromAZip(ofd.FileName);

            Text = $"Test set: {ofd.FileName}";
            lastPathLoaded = ofd.FileName;
            Init(set);
        }

        string lastPathLoaded;

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void fontMatcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontMatcher f = new FontMatcher();
            f.MdiParent = MdiParent;
            f.Show();
        }

        private void searchDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchDebug f = new SearchDebug();
            f.MdiParent = MdiParent;
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
                listView1.Items.Add(new ListViewItem(new string[] { item.Name, string.Empty, "" }) { Tag = item });
            }
        }
        public TestSet Set => set;

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            RunLocal(set.Tests.ToArray());
        }

        public SetRunContext RunLocal(IAutoTest[] tests)
        {
            TestReport report = new TestReport();
            var trun = new SetRunContext(set);
            report.Shown += (s, e) =>
            {
                Compiler.CompileAndGetInstance<ISetRun>(Set.StartupScript)?.Run(trun);

                report.Run(async (item) =>
                {
                    return item.Run();
                }, () =>
                {
                    Compiler.CompileAndGetInstance<ISetRun>(Set.FinalizerScript)?.Run(trun);
                });
            };
            report.MdiParent = MdiParent;

            report.Init(set, $"Report testing (test set: {lastPathLoaded})  ", tests);
            report.Show();
            return trun;
        }

        private void addTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }

        private void DeleteSelected()
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            if (MessageBox.Show($"Are you sure to delete selected tests ({listView1.SelectedItems.Count})?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                var test = listView1.SelectedItems[i].Tag as IAutoTest;
                Set.Tests.Remove(test);
            }
            UpdateTestsList();
        }

        private void editSelected()
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var test = listView1.SelectedItems[0].Tag as IAutoTest;
            if (test is SpawnableAutoTest sa)
            {
                Form1 f = new Form1();
                f.MdiParent = MdiParent;
                f.Init(sa);
                f.Show();
            }
            if (test is AutoTest at)
            {
                SimpleTestEditor f = new SimpleTestEditor();
                f.MdiParent = MdiParent;
                f.Init(at);
                f.Show();
            }
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

            var test = listView1.SelectedItems[0].Tag as IAutoTest;
            if (test is SpawnableAutoTest sat)
            {
                var d = DialogHelpers.StartDialog();

                d.AddStringField("name", "Name", test.Name);
                d.AddBoolField("emitter", "Use emitter", sat.UseEmitter);
                d.AddEnumField("faction", "Failed action", sat.FailedAction);

                if (!d.ShowDialog())
                    return;

                test.Name = d.GetStringField("name");
                sat.UseEmitter = d.GetBoolField("emitter");
                sat.FailedAction = d.GetEnumField<TestFailedBehaviour>("faction");
            }
            if (test is AutoTest ati)
            {
                var d = DialogHelpers.StartDialog();

                d.AddStringField("name", "Name", test.Name);
                d.AddEnumField("faction", "Failed action", ati.FailedAction);

                if (!d.ShowDialog())
                    return;

                test.Name = d.GetStringField("name");
                ati.FailedAction = d.GetEnumField<TestFailedBehaviour>("faction");
            }
            UpdateTestsList();
        }

        private void variablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var test = listView1.SelectedItems[0].Tag as IAutoTest;
            VariablesEditor ve = new VariablesEditor();
            ve.Init(test);
            ve.ShowDialog();
        }

        private void exportReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //form for custom report?
            //StringBuilder sb = new StringBuilder();
            //int failed = 0;
            //int total = 0;
            //sb.AppendLine($"Param;Duration (sec);State");
            //for (int i = 0; i < listView2.Items.Count; i++)
            //{
            //    var et = listView2.Items[i].Tag as EmittedSubTest;
            //    if (et.State == TestStateEnum.Failed)
            //        failed++;

            //    total++;
            //    sb.AppendLine($"{et.Data[et.Data.Keys.First()]};{et.Duration.TotalSeconds};{et.State}");
            //}
            //sb.AppendLine("");
            //sb.AppendLine($"Total;{total};Failed;{failed}");

            //SaveFileDialog sfd = new SaveFileDialog();
            //if (sfd.ShowDialog() != DialogResult.OK)
            //    return;

            //File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.Default);

            //if (Helpers.Question("Open report?", Text))
            //{
            //    Process.Start(sfd.FileName);
            //}
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Delete)
                DeleteSelected();

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
                return;

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

            var test = listView1.SelectedItems[0].Tag as IAutoTest;

            if (test is SpawnableAutoTest s)
            {
                if (s.lastContext != null && s.lastContext.WrongState != null)
                {
                    toolStripStatusLabel1.Text = $"wrong state: {s.lastContext.WrongState.Id}  {s.lastContext.WrongState.GetType().Name}";
                }
                if (s.lastContext != null)
                    updateSubTestList(s.lastContext);
            }
            else
            if (test is AutoTest ss)
            {
                if (ss.lastContext != null && ss.lastContext.WrongState != null)
                {
                    toolStripStatusLabel1.Text = $"wrong state: {ss.lastContext.WrongState.Id}  {ss.lastContext.WrongState.GetType().Name}";
                }
                if (ss.lastContext != null)
                    updateSubTestList(ss.lastContext);
            }

        }
        private void updateSubTestList(TestRunContext lastContext)
        {
            //listView2.Items.Clear();
            //foreach (var item in lastContext.SubTests)
            //{
            //    var lvi = new ListViewItem(new string[] { "sub", item.State.ToString(), item.Duration.TotalSeconds + "s", item.FinishTime.ToLongTimeString() }) { Tag = item };
            //    listView2.Items.Add(lvi);

            //    if (item.State == TestStateEnum.Failed)
            //    {
            //        lvi.BackColor = Color.Red;
            //        lvi.ForeColor = Color.White;
            //    }
            //    if (item.State == TestStateEnum.Success)
            //    {
            //        lvi.BackColor = Color.LightGreen;
            //        lvi.ForeColor = Color.Black;
            //    }
            //}
        }

        private void cloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var test = listView1.SelectedItems[0].Tag as IAutoTest;
            var clone = test.Clone();
            clone.Name += "_clone";
            set.Tests.Add(clone);

            UpdateTestsList();
        }

        public void RunRemotely(IAutoTest[] tests)
        {
            var d = AutoDialog.DialogHelpers.StartDialog();
            d.AddStringField("ip", "IP", "127.0.0.1");
            d.AddIntegerNumericField("port", "Port", 8888, 100000, 10);

            if (!d.ShowDialog())
                return;

            var ip = d.GetStringField("ip");
            var port = d.GetIntegerNumericField("port");
            var s1 = $"TEST_SET={Convert.ToBase64String(Encoding.Default.GetBytes(set.ToXml().ToString()))}";

            TestReport report = new TestReport();
            report.Shown += async (s, e) =>
            {
                TcpClient client = new TcpClient();
                await client.ConnectAsync(ip, port);
                var stream = client.GetStream();
                var wr = new StreamWriter(stream);
                var rdr = new StreamReader(stream);

                await wr.WriteLineAsync(s1);
                await wr.FlushAsync();
                var res = await rdr.ReadLineAsync();

                await wr.WriteLineAsync($"START_TEST_SET");
                await wr.FlushAsync();
                await rdr.ReadLineAsync();

                report.Run(async (item) =>
                {
                    var testIdx = set.Tests.IndexOf(item);
                    return await RemoteRunner.RunRemotely(wr, rdr, testIdx);
                }, async () =>
                {
                    await wr.WriteLineAsync($"FINISH_TEST_SET");
                    await wr.FlushAsync();
                    await rdr.ReadLineAsync();

                });

            };

            report.MdiParent = MdiParent;
            report.Init(set, $"Report testing (test set: {lastPathLoaded})  ", tests.ToArray());
            report.Show();
        }
        private async void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            RunRemotely(set.Tests.ToArray());
        }

        private void somplToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Set.Tests.Add(new AutoTest(Set) { Name = "new test1" });
            UpdateTestsList();
        }

        private void spawnableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Set.Tests.Add(new SpawnableAutoTest(Set) { Name = "new spawnable test1" });
            UpdateTestsList();
        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            ResourcesDialog rd = new ResourcesDialog();
            rd.MdiParent = MdiParent;
            rd.Init(set);
            rd.Show();
        }

        private void runSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void localToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            List<IAutoTest> tests = new();

            for (int i = 0; i < listView1.SelectedItems.Count; i++)
                tests.Add(listView1.SelectedItems[i].Tag as IAutoTest);

            RunLocal(tests.ToArray());
        }

        private void remotelyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            List<IAutoTest> tests = new();
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                var t = listView1.SelectedItems[i].Tag as IAutoTest;
                tests.Add(t);
            }

            RunRemotely(tests.ToArray());
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            VariablesEditor ved = new VariablesEditor();
            ved.Init(set);
            ved.Show();
        }

        private void setupCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScriptTextEditor ted = new();
            ted.TextChanged += () =>
            {
                set.StartupScript = ted.Editor.Text;
            };
            ted.Init(set.StartupScript);
            ted.MdiParent = MdiParent;
            ted.Show();
        }

        private void beforeEachTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScriptTextEditor ted = new();
            ted.TextChanged += () =>
            {
                set.BeforeTestScript = ted.Editor.Text;
            };
            ted.Init(set.BeforeTestScript);
            ted.MdiParent = MdiParent;
            ted.Show();
        }

        private void afterEachTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScriptTextEditor ted = new();
            ted.TextChanged += () =>
            {
                set.AfterTestScript = ted.Editor.Text;
            };
            ted.Init(set.AfterTestScript);
            ted.MdiParent = MdiParent;
            ted.Show();
        }

        private void finalizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScriptTextEditor ted = new();
            ted.TextChanged += () =>
            {
                set.FinalizerScript = ted.Editor.Text;
            };
            ted.Init(set.FinalizerScript);
            ted.MdiParent = MdiParent;
            ted.Show();
        }
    }
}
