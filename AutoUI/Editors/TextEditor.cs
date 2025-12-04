using AutoUI.Common;
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
            Editor.TextEditor.TextChanged += TextEditor_TextChanged;
        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke();
        }

        public void Init(string text)
        {
            Editor.Text = text;
        }
        public CodeEditor.CodeEditor Editor;

        public event Action Save;
        public event Action TextChanged;
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Save?.Invoke();
        }

        private void defaultITestRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.Text = DefaultScripts.DefaultTestSetScript;
        }
    }
}
