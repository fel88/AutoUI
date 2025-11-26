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
        public void Run(Func<IAutoTest, Task<AutoTestRunContext>> run)
        {
            Thread th = new(async () =>
            {
                foreach (var item in Tests)
                {
                    ListViewItem lvi = null;
                    listView1.Invoke((Action)(() =>
                    {
                        listView1.Items.Add(lvi = new ListViewItem(new string[] { item.Name,
                            item.State.ToString(),
                            string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        })
                        { Tag = item });
                    }));

                    var sw = Stopwatch.StartNew();

                    var res = await run(item);
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
                        lvi.SubItems[3].Text = duration.ToString();
                        lvi.SubItems[4].Text = res.CodePointer.ToString();
                        lvi.SubItems[5].Text = res.WrongState == null ? string.Empty : res.WrongState.ToString();

                    }));

                }
            })
            {
                IsBackground = true
            };
            th.Start();


        }

    }
}
