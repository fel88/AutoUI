using AutoDialog.Extensions;
using AutoUI.Common;
using AutoUI.Common.TestItems;
using AutoUI.Editors;
using AutoUI.TestItems;
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
            listView1.DragDrop += ListView1_DragDrop;
            listView1.ItemDrag += ListView1_ItemDrag;
            listView1.DragEnter += ListView1_DragEnter;
            listView1.DragOver += myListView_DragOver;
            listView1.DragLeave += myListView_DragLeave;
            listView1.AllowDrop = true;
            listView1.InsertionMark.Color = Color.Green;
        }


        // Moves the insertion mark as the item is dragged.
        private void myListView_DragOver(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the mouse pointer.
            Point targetPoint =
                listView1.PointToClient(new Point(e.X, e.Y));

            // Retrieve the index of the item closest to the mouse pointer.
            int targetIndex = listView1.InsertionMark.NearestIndex(targetPoint);

            // Confirm that the mouse pointer is not over the dragged item.
            if (targetIndex > -1)
            {
                // Determine whether the mouse pointer is to the left or
                // the right of the midpoint of the closest item and set
                // the InsertionMark.AppearsAfterItem property accordingly.
                Rectangle itemBounds = listView1.GetItemRect(targetIndex);
                if (targetPoint.X > itemBounds.Left + (itemBounds.Width / 2))
                {
                    listView1.InsertionMark.AppearsAfterItem = true;
                }
                else
                {
                    listView1.InsertionMark.AppearsAfterItem = false;
                }
            }

            // Set the location of the insertion mark. If the mouse is
            // over the dragged item, the targetIndex value is -1 and
            // the insertion mark disappears.
            listView1.InsertionMark.Index = targetIndex;
        }
        // Removes the insertion mark when the mouse leaves the control.
        private void myListView_DragLeave(object sender, EventArgs e)
        {
            listView1.InsertionMark.Index = -1;
        }
        private void ListView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void ListView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listView1.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void ListView1_DragDrop(object sender, DragEventArgs e)
        {

            // Retrieve the index of the insertion mark;
            int targetIndex = listView1.InsertionMark.Index;

            // If the insertion mark is not visible, exit the method.
            if (targetIndex == -1)
            {
                return;
            }

            // If the insertion mark is to the right of the item with
            // the corresponding index, increment the target index.
            if (listView1.InsertionMark.AppearsAfterItem)
            {
                targetIndex++;
            }

            // Retrieve the dragged item.
            ListViewItem draggedItem =
                (ListViewItem)e.Data.GetData(typeof(ListViewItem));

            // Insert a copy of the dragged item at the target index.
            // A copy must be inserted before the original item is removed
            // to preserve item index values. 
            var ati = draggedItem.Tag as AutoTestItem;
            currentCodeSection.Items.Insert(targetIndex, ati);
            for (int i = 0; i < currentCodeSection.Items.Count; i++)
            {
                if (i == targetIndex)
                    continue;
                if (currentCodeSection.Items[i] == ati)
                {
                    currentCodeSection.Items.RemoveAt(i);
                    break;
                }


            }
            listView1.Items.Insert(
                targetIndex, (ListViewItem)draggedItem.Clone());

            // Remove the original copy of the dragged item.

            listView1.Items.Remove(draggedItem);


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
            currentCodeSection = test.Main;
            if (currentCodeSection != null)
            {
                UpdateTestItemsList();
            }
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
            if (listView1.SelectedItems.Count == 0)
                return;

            currentItem = listView1.SelectedItems[0].Tag as AutoTestItem;
            var editorType = FindEditorType(currentItem);

            //pictureBox1.Image = null;
            //if (currentItem.GetType().GetCustomAttribute(typeof(TestItemEditorAttribute)) != null)
            if (editorType != null)
            {
                //   var t = currentItem.GetType().GetCustomAttribute(typeof(TestItemEditorAttribute)) as TestItemEditorAttribute;
                //var tie = Activator.CreateInstance(t.Editor) as ITestItemEditor;
                var tie = Activator.CreateInstance(editorType) as ITestItemEditor;
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

        private Type FindEditorType(AutoTestItem currentItem)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(z => z.GetCustomAttribute(typeof(TestItemEditorAttribute)) != null).ToArray();
            foreach (var item in types)
            {
                var p = item.GetCustomAttribute<TestItemEditorAttribute>();
                if (p.Target == currentItem.GetType())
                    return item;
            }
            return null;
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

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                listView1.SelectedItems[i].BackColor = Color.White;

                currentItem = listView1.SelectedItems[i].Tag as AutoTestItem;
                var result = currentItem.Process(new AutoTestRunContext() { Test = test });
                if (result == TestItemProcessResultEnum.Failed)
                {
                    listView1.SelectedItems[i].BackColor = Color.Red;
                    break;
                }
            }
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

        private void addToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            addToolStripMenuItem.Enabled = currentCodeSection != null;

        }
    }

}