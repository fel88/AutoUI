using AutoUI.Common;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "processTerminate")]
    public class ProcessTerminateTestItem : AutoTestItem
    {
        public override TestItemProcessResultEnum Process(TestRunContext ctx)
        {
            var p = System.Diagnostics.Process.GetProcessById((int)ctx.Vars[RegisterKey]);
            p.Kill(true);            
            return TestItemProcessResultEnum.Success;
        }

        public override string ToString()
        {
            return $"terminate process (register key: {RegisterKey})";
        }

        public override void ParseXml(IAutoTest set, XElement item)
        {
            if (item.Attribute("registerKey") != null)
                RegisterKey = item.Attribute("registerKey").Value;

            base.ParseXml(set, item);
        }
                
        
        public string RegisterKey { get; set; }
        public override string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<processTerminate registerKey=\"{RegisterKey}\">");
            sb.AppendLine("</processTerminate>");
            return sb.ToString();

        }
    }
}
