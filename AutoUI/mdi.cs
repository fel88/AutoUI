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

        public static TestSet set = new TestSet();
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

        private void fontMatcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontMatcher f = new FontMatcher();
            f.MdiParent = this;
            f.Show();
        }

        private void searchDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchDebug f = new SearchDebug();
            f.MdiParent = this;
            f.Show();
        }
    }    
}
