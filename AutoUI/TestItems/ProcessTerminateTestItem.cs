using System.Text;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "processTerminate")]
    public class ProcessTerminateTestItem : AutoTestItem
    {
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            var p = System.Diagnostics.Process.GetProcessById((int)ctx.Vars[RegisterKey]);
            p.Kill();
            return TestItemProcessResultEnum.Success;
        }

        public override void ParseXml(TestSet set, XElement item)
        {
            if (item.Attribute("registerKey") != null)
                RegisterKey = item.Attribute("registerKey").Value;

            base.ParseXml(set, item);
        }

        public string RegisterKey { get; set; }
        internal override string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<processTerminate registerKey=\"{RegisterKey}\">");
            sb.AppendLine("</processTerminate>");
            return sb.ToString();

        }
    }
}
