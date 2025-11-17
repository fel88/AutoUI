using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.Common.TestItems
{
    [XmlParse(XmlKey = "custom")]

    public class CompilingTestItem : AutoTestItem
    {
        public CompilingTestItem()
        {
            
        }

        public string ProgramText= @"
using AutoUI.Common;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

class Program{

public void run(AutoTestRunContext ctx){

}
}";

        public RoslynCompilerResults compile()
        {
            return Compiler.compile(ProgramText);
        }

        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            var results = compile();


            foreach (var item in results.Errors)
            {
                return TestItemProcessResultEnum.Failed;
            }
            try
            {
                Assembly asm = results.Assembly;

                Type[] allTypes = asm.GetTypes();

                foreach (Type t in allTypes.Take(1))
                {
                    var inst = Activator.CreateInstance(t);
                    //dynamic v = inst;
                    var mf = t.GetMethods().FirstOrDefault(z => z.Name.Contains("sum"));
                    var mf2 = t.GetMethods().FirstOrDefault(z => z.Name.Contains("run"));
                    if (mf != null)
                    {
                        var res = mf.Invoke(inst, new object[] { 3, 5 });
                        //MessageBox.Show(res + "");
                    }
                    if (mf2 != null)
                    {

                        mf2.Invoke(inst, new object[] { ctx });
                        //MessageBox.Show(ctx.Vars["temp"] + "");
                    }
                    else
                    {
                        return TestItemProcessResultEnum.Failed;//not found entry point
                    }
                    //MessageBox.Show(v.sum(3, 5));
                    //TryLoadCompiledType(res, t.ToString());
                    //Debug.WriteLine(t.ToString());
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return TestItemProcessResultEnum.Failed;
            }

            return TestItemProcessResultEnum.Success;
        }

        public override void ParseXml(IAutoTest parent, XElement item)
        {
            ProgramText = item.Value;

            base.ParseXml(parent, item);
        }


        public override string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<custom>");
            sb.AppendLine($"<![CDATA[{ProgramText}]]>");
            sb.AppendLine("</custom>");
            return sb.ToString();
        }
    }
}
