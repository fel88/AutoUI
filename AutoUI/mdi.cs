using System;
using System.Windows.Forms;

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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            EnvironmentEditor f = new EnvironmentEditor();
            f.MdiParent = this;
            f.Show();
        }
    }
}
