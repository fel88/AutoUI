using AutoUI.CodeEditor;
using AutoUI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace AutoUI.Editors
{
    public partial class ScriptTextEditor : Form
    {
        public ScriptTextEditor()
        {
            InitializeComponent();
            ElementHost elementHost = new ElementHost();
            elementHost.Dock = DockStyle.Fill;
            elementHost.Child = Editor = new CodeEditor.CodeEditor();
            panel1.Controls.Add(elementHost);
            Editor.TextEditor.TextChanged += TextEditor_TextChanged;


            lv = new ListView();
            lv.GridLines = true;
            lv.FullRowSelect = true;
            lv.View = View.Details;
            lv.Columns.Add("Line", 100);
            lv.Columns.Add("b", 100);
            lv.Columns.Add("c", 100);
            errorPanel.Controls.Add(lv, 0, 0);
            lv.Dock = DockStyle.Fill;
            panel1.Controls.Add(errorPanel);
            errorPanel.Dock = DockStyle.Bottom;
            errorPanel.Visible = false;
        }

        ListView lv;

        TableLayoutPanel errorPanel = new TableLayoutPanel();
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                var results = Compiler.Compile(Editor.Text);

                errorPanel.Visible = false;

                lv.Items.Clear();
                foreach (var item in results.Errors)
                {
                    errorPanel.Visible = true;
                    lv.Items.Add(new ListViewItem([$"{item.Line}", $"{item.Line}: {item.Text}"]) { Tag = item, BackColor = Color.Pink, ForeColor = Color.White });
                }
                if (results.Assembly != null)
                {
                    Assembly asm = results.Assembly;
                    Type[] allTypes = asm.GetTypes();

                    foreach (Type t in allTypes.Take(1))
                    {
                        var inst = Activator.CreateInstance(t);
                    }

                    MessageBox.Show("No errors", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            Editor.Text = Editor.Text.NormalizeCodeWithRoslyn();
        }
    }
}
