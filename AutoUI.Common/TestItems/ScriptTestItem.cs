using Microsoft.Win32;
using System.Reflection;
using System.Reflection.Emit;
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
        public string Program = DefaultScripts.DefaultTestScript;

        public RoslynCompilerResults Compile()
        {            
            return Compiler.Compile(Program);
        }

        public override string ToString()
        {
            return $"script ({Name})";
        }

        IRun Generator = null;
        string LastGeneratedProgramHash;

        private TestItemProcessResultEnum Run(IRun generator, TestRunContext ctx)
        {
            if (FailedOnException)
            {
                try
                {
                    Generator.Run(ctx);
                }
                catch (Exception ex)
                {
                    return TestItemProcessResultEnum.Failed;
                }
            }
            else
                Generator.Run(ctx);

            return TestItemProcessResultEnum.Success;

        }
        public override TestItemProcessResultEnum Process(TestRunContext ctx)
        {
            var hash1 = Program.MD5Hash();
            if (Generator != null && hash1 == LastGeneratedProgramHash)
            {
                try
                {
                    return Run(Generator, ctx);

                }
                catch (Exception ex)
                {
                    return TestItemProcessResultEnum.Failed;

                }

            }
            Generator = null;

            var results = Compile();


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
                    var inst = Generator = (IRun)Activator.CreateInstance(t);
                    //dynamic v = inst;                    
                    LastGeneratedProgramHash = Program.MD5Hash();

                    if (inst == null)
                        return TestItemProcessResultEnum.Failed;//not found entry point

                    return Run(Generator, ctx);
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
            Program = item.Value;
            if (item.Attribute("failedOnException") != null)
                FailedOnException = bool.Parse(item.Attribute("failedOnException").Value);

            base.ParseXml(parent, item);
        }


        public override string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<script name=\"{Name}\" failedOnException=\"{FailedOnException}\">");
            sb.AppendLine($"<![CDATA[{Program}]]>");
            sb.AppendLine("</script>");
            return sb.ToString();
        }
    }
}
