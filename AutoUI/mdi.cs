using AutoUI.TestItems;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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

        TestSet set = new TestSet();
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            EnvironmentEditor f = new EnvironmentEditor();
            f.Init(set);
            f.MdiParent = this;
            f.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            PatternsEditor f = new PatternsEditor();
            f.Init(set.Pool);
            f.MdiParent = this;
            f.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<root>");
            foreach (var test in set.Tests)
            {
                sb.AppendLine($"<test id=\"{test.Id}\" name=\"{test.Name}\">");

                foreach (var item in test.Items)
                {
                    sb.AppendLine(item.ToXml());
                }
                sb.AppendLine("</test>");
            }

            set.Pool.ToXml(sb);

            sb.AppendLine("</root>");
            sb.ToString();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "xml files|*.xml";
            if (sfd.ShowDialog() != DialogResult.OK) return;
            File.WriteAllText(sfd.FileName, sb.ToString());
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Xml files|*.xml";
            if (ofd.ShowDialog() != DialogResult.OK) return;

            set = new TestSet();
            var doc = XDocument.Load(ofd.FileName);
            var root = doc.Descendants("root").First();
            if (root.Element("pool") != null)
                set.Pool.ParseXml(set, root.Element("pool"));

            foreach (var titem in root.Descendants("test"))
            {
                var test = new AutoTest(set);
                test.ParseXml(titem);               

                set.Tests.Add(test);
                //get all types
                Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(z => z.GetCustomAttribute(typeof(XmlParseAttribute)) != null).ToArray();
                foreach (var item in titem.Elements())
                {
                    var fr = types.FirstOrDefault(z => (z.GetCustomAttribute(typeof(XmlParseAttribute)) as XmlParseAttribute).XmlKey == item.Name);
                    if (fr != null)
                    {
                        var tp = Activator.CreateInstance(fr) as AutoTestItem;
                        tp.ParseXml(set, item);
                        test.Items.Add(tp);
                    }
                }
            }
            EnvironmentEditor f = new EnvironmentEditor();
            f.Init(set);
            f.MdiParent = this;
            f.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            AboutBox1 b = new AboutBox1();
            b.ShowDialog();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
