using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace AutoUI.Editors
{
    public partial class TextEditor : Form
    {
        public TextEditor()
        {
            InitializeComponent();
            ElementHost elementHost = new ElementHost();
            elementHost.Dock = DockStyle.Fill;
            elementHost.Child = Editor = new CodeEditor.CodeEditor();
            panel1.Controls.Add(elementHost);
        }

        public void Init(string text)
        {
            Editor.Text = text;
        }
        public CodeEditor.CodeEditor Editor;

        public event Action Save;
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Save?.Invoke();
        }
    }
}
