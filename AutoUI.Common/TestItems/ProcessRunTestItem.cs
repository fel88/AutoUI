using AutoUI.Common;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.Common.TestItems
{
    [XmlParse(XmlKey = "processRun")]
    public class ProcessRunTestItem : AutoTestItem
    {
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = RunFromVar ? (string)ctx.Vars[Var] : ExePath;

            if (!File.Exists(p.StartInfo.FileName))
                return TestItemProcessResultEnum.Failed;

            p.StartInfo.WorkingDirectory = new FileInfo(p.StartInfo.FileName).Directory.FullName;
            p.Start();

            if (StorePIDToRegister)            
                ctx.Vars.Add(RegisterKey, p.Id);
            
            return TestItemProcessResultEnum.Success;
        }

        public override void ParseXml(AutoTest set, XElement item)
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
        public override string ToXml()
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
