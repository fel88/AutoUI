using AutoDialog.Extensions;
using AutoUI.TestItems;
using AutoUI.TestItems.Editors;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace AutoUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mf = new MessageFilter();
            Application.AddMessageFilter(mf);
        }
        MessageFilter mf = null;

        AutoTest test;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var ee = new KeyEventArgs(keyData);
            if (ee.Control && ee.KeyCode == Keys.V)
            {
                /* if (currentItem != null && currentItem is SearchByPatternImage s)
                {
                    s.Pattern = Clipboard.GetImage() as Bitmap;

                    var temp = pictureBox1.Image;
                    pictureBox1.Image = s.Pattern.Clone() as Bitmap;
                    if (temp != null)
                    {
                        temp.Dispose();
                    }
                }
                else
                {
                    pictureBox1.Image = Clipboard.GetImage() as Bitmap;
                 }*/
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        internal void Init(AutoTest test)
        {
            this.test = test;
            currentCodeSection = test.Main ?? test.Sections.First();
            UpdateTestItemsList();
            UpdateSectionsList();
        }

        public Bitmap GetScreenshot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }

            return bitmap;
        }

        public void UpdateTestItemsList()
        {
            listView1.Items.Clear();
            foreach (var t in currentCodeSection.Items)
            {
                listView1.Items.Add(new ListViewItem(new string[] { t.GetType().Name }) { Tag = t });
            }
        }

        private void searchPatternImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentCodeSection.Items.Add(new SearchByPatternImage());
            UpdateTestItemsList();
        }

        AutoTestItem currentItem = null;
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            currentItem = listView1.SelectedItems[0].Tag as AutoTestItem;

            //pictureBox1.Image = null;
            if (currentItem.GetType().GetCustomAttribute(typeof(TestItemEditorAttribute)) != null)
            {
                var t = currentItem.GetType().GetCustomAttribute(typeof(TestItemEditorAttribute)) as TestItemEditorAttribute;
                var tie = Activator.CreateInstance(t.Editor) as ITestItemEditor;
                tie.Init(currentItem);

                var del = tableLayoutPanel1.Controls.OfType<ITestItemEditor>().FirstOrDefault();
                if (del != null)
                {
                    tableLayoutPanel1.Controls.Remove(del as Control);
                }
                tableLayoutPanel1.Controls.Add(tie as Control, 0, 0);
                (tie as Control).Dock = DockStyle.Fill;
            }
        }

        void deleteSelected()
        {
            if (listView1.SelectedItems.Count == 0) return;
            currentItem = listView1.SelectedItems[0].Tag as AutoTestItem;
            if (MessageBox.Show($"sure to delete: {listView1.SelectedItems.Count}?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    var tt = listView1.SelectedItems[i].Tag as AutoTestItem;
                    currentCodeSection.Items.Remove(tt);
                }

                UpdateTestItemsList();
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteSelected();
        }

        private void clickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentCodeSection.Items.Add(new ClickAutoTestItem());
            UpdateTestItemsList();
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentCodeSection.Items.Clear();
            UpdateTestItemsList();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("<?xml version=\"1.0\"?>");
            //sb.AppendLine("<root>");
            //foreach (var item in test.Items)
            //{
            //    sb.AppendLine(item.ToXml());
            //}
            //sb.AppendLine("</root>");
            //sb.ToString();

            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "xml files|*.xml";
            //if (sfd.ShowDialog() != DialogResult.OK) return;
            //File.WriteAllText(sfd.FileName, sb.ToString());
        }

        private void delayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentCodeSection.Items.Add(new DelayAutoTestItem());
            UpdateTestItemsList();
        }

        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var ind1 = listView1.Items.IndexOf(listView1.SelectedItems[0]);
            var elem = currentCodeSection.Items[ind1];

            if (ind1 > 0)
            {
                currentCodeSection.Items.RemoveAt(ind1);
                currentCodeSection.Items.Insert(ind1 - 1, elem);
            }
            UpdateTestItemsList();

        }

        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var ind1 = listView1.Items.IndexOf(listView1.SelectedItems[0]);
            var elem = currentCodeSection.Items[ind1];
            if (ind1 < listView1.Items.Count - 1)
            {
                currentCodeSection.Items.RemoveAt(ind1);
                currentCodeSection.Items.Insert(ind1 + 1, elem);
            }
            UpdateTestItemsList();
        }

        private void startDragToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mouseUpdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentCodeSection.Items.Add(new MouseUpDownTestItem());
            UpdateTestItemsList();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = "Xml files|*.xml";
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    test = new AutoTest();
            //    var doc = XDocument.Load(ofd.FileName);
            //    var root = doc.Descendants("root").First();

            //    //get all types
            //    Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(z => z.GetCustomAttribute(typeof(XmlParseAttribute)) != null).ToArray();
            //    foreach (var item in root.Elements())
            //    {
            //        var fr = types.FirstOrDefault(z => (z.GetCustomAttribute(typeof(XmlParseAttribute)) as XmlParseAttribute).XmlKey == item.Name);
            //        if (fr != null)
            //        {
            //            var tp = Activator.CreateInstance(fr) as AutoTestItem;
            //            tp.ParseXml(item);
            //            test.Items.Add(tp);
            //        }
            //    }
            //}

            //UpdateTestItemsList();
        }

        private void searchByPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentCodeSection.Items.Add(new SearchByPatternImage());
            UpdateTestItemsList();
        }

        private void clickToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            addOrInsertItem(new ClickAutoTestItem());
        }

        private void delayToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            currentCodeSection.Items.Add(new DelayAutoTestItem());
            UpdateTestItemsList();
        }

        private void mouseUpDownToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            currentCodeSection.Items.Add(new MouseUpDownTestItem());
            UpdateTestItemsList();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(() =>
            {
                listView1.Invoke((Action)(() =>
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {

                        listView1.Items[i].BackColor = Color.White;
                    }
                }));
                var sw = Stopwatch.StartNew();
                var ctx = test.Run();
                sw.Stop();
                listView1.Invoke((Action)(() =>
                {
                    toolStripStatusLabel1.Text = "test took: " + sw.ElapsedMilliseconds + "ms";
                }));

                if (ctx.WrongState != null)
                {
                    listView1.Invoke((Action)(() =>
                    {
                        for (int i = 0; i < listView1.Items.Count; i++)
                        {
                            if (listView1.Items[i].Tag == ctx.WrongState)
                            {
                                listView1.Items[i].BackColor = Color.Red;
                            }
                        }
                    }));
                }
            });
            th.IsBackground = true;
            th.Start();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void setPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            currentItem = listView1.SelectedItems[0].Tag as AutoTestItem;

            if (currentItem is SearchByPatternImage b)
            {
                PatternSelector s = new PatternSelector();
                s.Init(test.Parent.Pool);
                s.ShowDialog();
                if (s.Selected != null)
                {
                    b.Pattern = s.Selected;
                }
            }
            if (currentItem is FindAllByPatternImage ff)
            {
                PatternSelector s = new PatternSelector();
                s.Init(test.Parent.Pool);
                s.ShowDialog();
                if (s.Selected != null)
                {
                    ff.Pattern = s.Selected;
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                deleteSelected();
            }
        }

        private void scriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentCodeSection.Items.Add(new CompilingTestItem());
            UpdateTestItemsList();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentCodeSection.Items.Add(new ProcessRunTestItem());
            UpdateTestItemsList();
        }

        CodeSection currentCodeSection = null;
        private void cursorPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentCodeSection.Items.Add(new CursorPositionTestItem());
            UpdateTestItemsList();
        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            currentItem = listView1.SelectedItems[0].Tag as AutoTestItem;
            var sw = Stopwatch.StartNew();
            currentItem.Process(new AutoTestRunContext() { Test = test });
            sw.Stop();
            toolStripStatusLabel1.Text = sw.ElapsedMilliseconds + "ms";
        }

        private void terminateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentCodeSection.Items.Add(new ProcessTerminateTestItem());
            UpdateTestItemsList();
        }



        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var ind1 = listView1.Items.IndexOf(listView1.SelectedItems[0]);
            var elem = currentCodeSection.Items[ind1];

            currentCodeSection.Items.RemoveAt(ind1);
            currentCodeSection.Items.Insert(0, elem);
            UpdateTestItemsList();
        }

        void addOrInsertItem(AutoTestItem ati)
        {
            ati.ParentTest = test;
            if (listView1.SelectedIndices.Count > 0)
            {
                var ind1 = listView1.SelectedIndices[0];
                currentCodeSection.Items.Insert(ind1, ati);
            }
            else
            {
                currentCodeSection.Items.Add(ati);
            }
            UpdateTestItemsList();
        }

        private void waitPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addOrInsertItem(new WaitPatternImage() { ParentTest = test });
        }

        private void gotoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            addOrInsertItem(new GotoAutoTestItem());
        }

        private void labelToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            addOrInsertItem(new LabelAutoTestItem());
        }

        private void screenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addOrInsertItem(new ScreenshotTestItem());
        }

        private void findAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addOrInsertItem(new FindAllByPatternImage());

        }

        private void iterateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addOrInsertItem(new Iterator());

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;


            var ati = listView1.SelectedItems[0].Tag as AutoTestItem;
            ati.EditWithAutoDialog();

            UpdateTestItemsList();

        }

        private void mainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeSection != test.Main && Helpers.Question("switch to main?", ParentForm.Text))
            {
                currentCodeSection = test.Main;
                sectionToolStripStatusLabel.Text = "main";
                UpdateTestItemsList();
            }
        }

        private void finalizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeSection != test.Finalizer && Helpers.Question("switch to finalizer?", ParentForm.Text))
            {
                currentCodeSection = test.Finalizer;
                sectionToolStripStatusLabel.Text = "finalizer";
                UpdateTestItemsList();
            }
        }

        private void emitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeSection != test.Emitter && Helpers.Question("switch to emitter?", ParentForm.Text))
            {
                currentCodeSection = test.Emitter;
                sectionToolStripStatusLabel.Text = "emitter";
                UpdateTestItemsList();
            }
        }

        private void wheelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addOrInsertItem(new MouseWheelAutoTestItem());
        }


        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
                return;

            var cs = listView2.SelectedItems[0].Tag as CodeSection;
            if (currentCodeSection != cs && Helpers.Question($"switch to {cs.Name}?", Text))
            {
                currentCodeSection = cs;
                sectionToolStripStatusLabel.Text = cs.Name;
                UpdateTestItemsList();
            }
        }

        public void UpdateSectionsList()
        {
            listView2.Items.Clear();
            foreach (var item in test.Sections)
            {
                listView2.Items.Add(new ListViewItem(new string[] { item.Name, item.Role.ToString() }) { Tag = item });
            }
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var d = AutoDialog.DialogHelpers.StartDialog();
            d.AddStringField("name", "Name");
            d.AddEnumField<CodeSectionRole>("role", "Role", CodeSectionRole.FsmState);

            if (!d.ShowDialog())
                return;

            test.Sections.Add(new CodeSection() { Name = d.GetStringField("name"), Role = d.GetEnumField<CodeSectionRole>("role") });
            UpdateSectionsList();

        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void edirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
                return;

            var cs = listView2.SelectedItems[0].Tag as CodeSection;

            var d = AutoDialog.DialogHelpers.StartDialog();
            d.AddStringField("name", "Name", cs.Name);
            d.AddEnumField<CodeSectionRole>("role", "Role", cs.Role);

            if (!d.ShowDialog())
                return;

            cs.Name = d.GetStringField("name");
            cs.Role = d.GetEnumField<CodeSectionRole>("role");
            UpdateSectionsList();

        }
    }

}