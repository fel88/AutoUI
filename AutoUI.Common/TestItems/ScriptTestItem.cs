using Microsoft.Win32;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.Common.TestItems
{
    [XmlParse(XmlKey = "script")]

    public class ScriptTestItem : AutoTestItem
    {
        public ScriptTestItem()
        {

        }
        public bool FailedOnException { get; set; } = true;
        public string ProgramText = @"
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

        public override string ToString()
        {
            return $"script";
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
                    var mf2 = t.GetMethods().FirstOrDefault(z => z.Name.Contains("run"));
                    
                    if (mf2 != null)
                    {
                        if (FailedOnException)
                        {
                            try
                            {
                                mf2.Invoke(inst, new object[] { ctx });
                            }
                            catch (Exception ex)
                            {
                                return TestItemProcessResultEnum.Failed;
                            }
                        }
                        else
                            mf2.Invoke(inst, new object[] { ctx });                        
                    }
                    else
                    {
                        return TestItemProcessResultEnum.Failed;//not found entry point
                    }
                    
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
            if (item.Attribute("failedOnException") != null)
                FailedOnException = bool.Parse(item.Attribute("failedOnException").Value);

            base.ParseXml(parent, item);
        }


        public override string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<script name=\"{Name}\" failedOnException=\"{FailedOnException}\">");
            sb.AppendLine($"<![CDATA[{ProgramText}]]>");
            sb.AppendLine("</script>");
            return sb.ToString();
        }
    }
}
