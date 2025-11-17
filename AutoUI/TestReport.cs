using AutoUI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            Shown += TestReport_Shown;
        }

        private void TestReport_Shown(object sender, EventArgs e)
        {
            Run();
        }

        public void Init(TestSet set, string captionPrefix)
        {
            Text = $"{captionPrefix}{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}";
            Set = set;
        }

        TestSet Set;
        public void Run()
        {
            Thread th = new(() =>
            {
                foreach (var item in Set.Tests)
                {
                    ListViewItem lvi = null;
                    listView1.Invoke((Action)(() =>
                    {
                        listView1.Items.Add(lvi = new ListViewItem(new string[] { item.Name, item.State.ToString(), "" }) { Tag = item });
                    }));

                    item.Reset();

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
