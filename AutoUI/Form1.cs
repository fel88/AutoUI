using AutoUI.TestItems;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AutoUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        AutoTest test = new AutoTest();
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var ee = new KeyEventArgs(keyData);
            if (ee.Control && ee.KeyCode == Keys.V)
            {
                if (currentItem != null && currentItem is SearchByPatternImage s)
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
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        internal void Init(AutoTest test)
        {
            this.test = test;
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
            foreach (var t in test.Items)
            {
                listView1.Items.Add(new ListViewItem(new string[] { t.GetType().Name }) { Tag = t });
            }
        }
        private void searchPatternImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test.Items.Add(new SearchByPatternImage());
            UpdateTestItemsList();
        }

        AutoTestItem currentItem = null;
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            currentItem = listView1.SelectedItems[0].Tag as AutoTestItem;
            propertyGrid1.SelectedObject = currentItem;
            if (currentItem is SearchByPatternImage b)
            {
                pictureBox1.Image = b.Pattern;
            }            
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            currentItem = listView1.SelectedItems[0].Tag as AutoTestItem;

            test.Items.Remove(currentItem);
            UpdateTestItemsList();

        }

        private void gotoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test.Items.Add(new GotoAutoTestItem());
            UpdateTestItemsList();
        }

        private void labelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test.Items.Add(new LabelAutoTestItem());
            UpdateTestItemsList();
        }

        

        private void clickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test.Items.Add(new ClickAutoTestItem());
            UpdateTestItemsList();
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test.Items.Clear();
            UpdateTestItemsList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<root>");
            foreach (var item in test.Items)
            {
                sb.AppendLine(item.ToXml());
            }
            sb.AppendLine("</root>");
            sb.ToString();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "xml files|*.xml";
            if (sfd.ShowDialog() != DialogResult.OK) return;
            File.WriteAllText(sfd.FileName, sb.ToString());
        }

        private void delayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test.Items.Add(new DelayAutoTestItem());
            UpdateTestItemsList();
        }

        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var ind1 = listView1.Items.IndexOf(listView1.SelectedItems[0]);
            var elem = test.Items[ind1];

            if (ind1 > 0)
            {
                test.Items.RemoveAt(ind1);
                test.Items.Insert(ind1 - 1, elem);
            }
            UpdateTestItemsList();

        }

        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var ind1 = listView1.Items.IndexOf(listView1.SelectedItems[0]);
            var elem = test.Items[ind1];
            if (ind1 < listView1.Items.Count - 1)
            {
                test.Items.RemoveAt(ind1);
                test.Items.Insert(ind1 + 1, elem);
            }
            UpdateTestItemsList();
        }

        private void startDragToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mouseUpdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test.Items.Add(new MouseUpDownTestItem());
            UpdateTestItemsList();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Xml files|*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                test = new AutoTest();
                var doc = XDocument.Load(ofd.FileName);
                var root = doc.Descendants("root").First();

                //get all types
                Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(z => z.GetCustomAttribute(typeof(XmlParseAttribute)) != null).ToArray();
                foreach (var item in root.Elements())
                {
                    var fr = types.FirstOrDefault(z => (z.GetCustomAttribute(typeof(XmlParseAttribute)) as XmlParseAttribute).XmlKey == item.Name);
                    if (fr != null)
                    {
                        var tp = Activator.CreateInstance(fr) as AutoTestItem;
                        tp.ParseXml(item);
                        test.Items.Add(tp);
                    }
                }
            }

            UpdateTestItemsList();
        }

        private void searchByPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test.Items.Add(new SearchByPatternImage());
            UpdateTestItemsList();
        }

        private void clickToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            test.Items.Add(new ClickAutoTestItem());
            UpdateTestItemsList();
        }

        private void delayToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            test.Items.Add(new DelayAutoTestItem());
            UpdateTestItemsList();
        }

        private void mouseUpDownToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            test.Items.Add(new MouseUpDownTestItem());
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

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
