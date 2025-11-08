using AutoUI.Common;
using AutoUI.Common.TestItems;
using AutoUI.Editors;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUI.TestItems.Editors
{
    [TestItemEditor(Target = typeof(CompilingTestItem))]
    public partial class CompilerTestItem : UserControl, ITestItemEditor
    {
        public CompilerTestItem()
        {
            InitializeComponent();
            errorPanel.Height = 100;

            lv = new ListView();
            lv.GridLines = true;
            lv.FullRowSelect = true;
            lv.View = View.Details;
            lv.Columns.Add("Line", 100);
            lv.Columns.Add("b", 100);
            lv.Columns.Add("c", 100);
            errorPanel.Controls.Add(lv, 0, 0);
            lv.Dock = DockStyle.Fill;
            richTextBox1.Controls.Add(errorPanel);
            errorPanel.Dock = DockStyle.Bottom;
            errorPanel.Visible = false;
        }
        ListView lv;
        TableLayoutPanel errorPanel = new TableLayoutPanel();
        public CompilingTestItem TestItem;
        public void Init(AutoTestItem item)
        {
            TestItem = item as CompilingTestItem;
            richTextBox1.Text = TestItem.ProgramText;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var results = TestItem.compile();

            errorPanel.Visible = false;
            
            lv.Items.Clear();
            foreach (var item in results.Errors.OfType<CompilerError>())
            {
                errorPanel.Visible = true;           
                lv.Items.Add(new ListViewItem(new string[] { item.Line+"", item.ErrorNumber + ": " + item.ErrorText }) { Tag = item, BackColor = Color.Pink, ForeColor = Color.White });
            }
            try
            {
                Assembly asm = results.CompiledAssembly;

                Type[] allTypes = results.CompiledAssembly.GetTypes();

                foreach (Type t in allTypes)
                {
                    var inst = Activator.CreateInstance(t);
                    //dynamic v = inst;
                    var mf = t.GetMethods().FirstOrDefault(z => z.Name.Contains("sum"));
                    var mf2 = t.GetMethods().FirstOrDefault(z => z.Name.Contains("run"));
                    if (mf != null)
                    {
                        var res = mf.Invoke(inst, new object[] { 3, 5 });
                        MessageBox.Show(res + "");
                    }
                    if (mf2 != null)
                    {
                        var ctx = new AutoTestRunContext();
                        mf2.Invoke(inst, new object[] { ctx });
                        MessageBox.Show(ctx.Vars["temp"] + "");
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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            TestItem.ProgramText = richTextBox1.Text;
        }
    }

}
