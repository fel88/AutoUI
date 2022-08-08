using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "processRun")]
    public class ProcessRunTestItem : AutoTestItem
    {
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            Process p = null;
            if (RunFromVar)
            {
                p = System.Diagnostics.Process.Start((string)ctx.Vars[Var]);
            }
            else
            {
                p = System.Diagnostics.Process.Start(ExePath);
            }
            if (StorePIDToRegister)
            {
                ctx.Vars.Add(RegisterKey, p.Id);
            }
            return TestItemProcessResultEnum.Success;
        }

        public override void ParseXml(TestSet set, XElement item)
        {
            ExePath = item.Element("exePath").Value;
            if (item.Attribute("registerKey") != null)
                RegisterKey = item.Attribute("registerKey").Value;
            if (item.Attribute("storeRegister") != null)
                StorePIDToRegister = bool.Parse(item.Attribute("storeRegister").Value);
            if (item.Attribute("runFromVar") != null)
                RunFromVar = bool.Parse(item.Attribute("runFromVar").Value);
            if (item.Attribute("var") != null)
                Var = item.Attribute("var").Value;
            base.ParseXml(set, item);
        }

        public string ExePath { get; set; }
        public bool RunFromVar { get; set; }
        public string Var { get; set; }
        public bool StorePIDToRegister { get; set; }
        public string RegisterKey { get; set; }
        internal override string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<processRun storeRegister=\"{StorePIDToRegister}\" registerKey=\"{RegisterKey}\" var=\"{Var}\" runFromVar=\"{RunFromVar}\">");
            sb.AppendLine("<exePath>");
            sb.AppendLine($"<![CDATA[{ExePath}]]>");
            sb.AppendLine("</exePath>");
            sb.AppendLine("</processRun>");
            return sb.ToString();

        }
    }
}
