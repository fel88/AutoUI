using AutoUI.Common;
using AutoUI.Common.TestItems;
using AutoUI.Editors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace AutoUI.TestItems.Editors
{
    [TestItemEditor(Target = typeof(CompilingTestItem))]
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
            TestItem.ProgramText = codeEditor.Text;
        }

        ListView lv;
        CodeEditor.CodeEditor codeEditor;
        TableLayoutPanel errorPanel = new TableLayoutPanel();
        public CompilingTestItem TestItem;
        public void Init(AutoTestItem item)
        {
            TestItem = item as CompilingTestItem;
            codeEditor.Text = TestItem.ProgramText;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var results = TestItem.compile();

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
                    var inst = Activator.CreateInstance(t);
                    //dynamic v = inst;
                    //var mf = t.GetMethods().FirstOrDefault(z => z.Name.Contains("sum"));
                    var mf2 = t.GetMethods().FirstOrDefault(z => z.Name.Contains("run"));
                    /*   if (mf != null)
                       {
                           var res = mf.Invoke(inst, new object[] { 3, 5 });
                           MessageBox.Show(res + "");
                       }*/
                    if (mf2 != null)
                    {
                        var ctx = new AutoTestRunContext();
                        mf2.Invoke(inst, new object[] { ctx });
                        //MessageBox.Show(ctx.Vars["temp"] + "");
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
               

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            codeEditor.Text = codeEditor.Text.NormalizeCodeWithRoslyn();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            var offset = codeEditor.TextEditor.CaretOffset;
            codeEditor.TextEditor.Document.Insert(offset, $"Debugger.Launch();{Environment.NewLine}");
        }
    }

}
