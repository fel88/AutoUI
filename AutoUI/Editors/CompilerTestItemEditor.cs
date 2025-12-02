using AutoUI.Common;
using AutoUI.Common.TestItems;
using AutoUI.Editors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualBasic;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace AutoUI.TestItems.Editors
{
    [TestItemEditor(Target = typeof(ScriptTestItem))]
    public partial class CompilerTestItemEditor : UserControl, ITestItemEditor
    {
        public CompilerTestItemEditor()
        {
            InitializeComponent();
            errorPanel.Height = 100;
            ElementHost host = new ElementHost();
            host.Child = codeEditor = new CodeEditor.CodeEditor();
            codeEditor.TextEditor.TextChanged += TextEditor_TextChanged;
            host.Dock = DockStyle.Fill;
            panel1.Controls.Add(host);


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

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            TestItem.Program = codeEditor.Text;
        }

        ListView lv;
        CodeEditor.CodeEditor codeEditor;
        TableLayoutPanel errorPanel = new TableLayoutPanel();
        public ScriptTestItem TestItem;
        public void Init(AutoTestItem item)
        {
            TestItem = item as ScriptTestItem;
            codeEditor.Text = TestItem.Program;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var results = TestItem.Compile();

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
                        var inst = (IRun)Activator.CreateInstance(t);
                    }

                    MessageBox.Show("No errors", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            codeEditor.Text = codeEditor.Text.NormalizeCodeWithRoslyn();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            var offset = codeEditor.TextEditor.CaretOffset;
            codeEditor.TextEditor.Document.Insert(offset, $"Debugger.Launch();{Environment.NewLine}");
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            var results = TestItem.Compile();

            errorPanel.Visible = false;

            lv.Items.Clear();
            foreach (var item in results.Errors)
            {
                errorPanel.Visible = true;
                lv.Items.Add(new ListViewItem([$"{item.Line}", $"{item.Line}: {item.Text}"]) { Tag = item, BackColor = Color.Pink, ForeColor = Color.White });
            }
            try
            {
                Assembly asm = results.Assembly;

                Type[] allTypes = results.Assembly.GetTypes();

                foreach (Type t in allTypes)
                {
                    var inst = (IRun)Activator.CreateInstance(t);
                    //dynamic v = inst;
                    //var mf = t.GetMethods().FirstOrDefault(z => z.Name.Contains("sum"));

                    /*   if (mf != null)
                       {
                           var res = mf.Invoke(inst, new object[] { 3, 5 });
                           MessageBox.Show(res + "");
                       }*/
                    if (inst != null)
                    {
                        var ctx = new AutoTestRunContext();
                        inst.Run(ctx);
                        //MessageBox.Show(ctx.Vars["temp"] + "");
                        break;
                    }
                    //MessageBox.Show(v.sum(3, 5));
                    //TryLoadCompiledType(res, t.ToString());
                    //Debug.WriteLine(t.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

}
