using AutoUI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUI
{
    public partial class TestReport : Form
    {
        public TestReport()
        {
            InitializeComponent();
        }


        public void Init(TestSet set, string captionPrefix, IAutoTest[] tests = null)
        {
            Text = $"{captionPrefix}{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}";
            Set = set;
            if (tests == null)
                Tests = set.Tests.ToArray();
            else
                Tests = tests;
        }

        IAutoTest[] Tests;
        TestSet Set;

        public class TestRunInfo
        {
            public TestRunContext Context;
            public IAutoTest Test;
        }

        public void Run(Func<IAutoTest, Task<TestRunContext>> run, Action finalize = null)
        {
            Thread th = new(async () =>
            {
                SetRunContext trun = new(Set);
                try
                {

                    foreach (var item in Tests)
                    {
                        ListViewItem lvi = null;
                        var tri = new TestRunInfo() { Test = item };
                        listView1.Invoke((Action)(() =>
                        {
                            listView1.Items.Add(lvi = new ListViewItem([
                                item.Name,
                                //item.State.ToString(),
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                            ])
                            { Tag = tri });
                        }));

                        var sw = Stopwatch.StartNew();

                        var res = await run(item);
                        tri.Context = res;
                        sw.Stop();

                        var duration = sw.ElapsedMilliseconds;

                        int cc = 0;
                        foreach (var sub in res.SubTests)
                        {
                            toolStripStatusLabel3.Visible = true;
                            toolStripStatusLabel3.Text = $"subtests: {cc} / {res.SubTests.Count}";
                            var res1 = sub.Run();
                            cc++;
                        }
                        toolStripStatusLabel3.Visible = false;

                        listView1.Invoke((Action)(() =>
                        {
                            if (res.State == TestStateEnum.Failed)
                            {
                                lvi.BackColor = Color.Red;
                                lvi.ForeColor = Color.White;
                            }
                            if (res.State == TestStateEnum.Success)
                            {
                                lvi.BackColor = Color.LightGreen;
                                lvi.ForeColor = Color.Black;
                            }
                            if (res.State == TestStateEnum.Emitter)
                            {
                                lvi.BackColor = Color.Violet;
                                lvi.ForeColor = Color.White;
                            }
                            lvi.SubItems[1].Text = res.State.ToString();
                            lvi.SubItems[2].Text = DateTime.Now.ToLongTimeString();
                            lvi.SubItems[3].Text = duration.ToString();
                            lvi.SubItems[4].Text = res.CodePointer.ToString();
                            lvi.SubItems[5].Text = res.WrongState == null ? string.Empty : res.WrongState.ToString();

                        }));

                    }
                }
                catch (Exception ex)
                {
                    Helpers.Error(ex.Message);
                }
                try
                {
                    finalize?.Invoke();
                }
                catch (Exception ex)
                {
                    Helpers.Error(ex.Message);

                }
            })
            {
                IsBackground = true
            };
            th.Start();


        }

        private void registersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var tri = listView1.SelectedItems[0].Tag as TestRunInfo;
            StringsListEditor s = new StringsListEditor();
            s.Init(tri.Context.StringRegisters, false);
            s.ShowDialog();
        }
    }
}
