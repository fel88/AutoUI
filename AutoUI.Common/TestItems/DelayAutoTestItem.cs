using AutoUI.Common;
using System.CodeDom;
using System.Threading;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "delay")]
    public class DelayAutoTestItem : AutoTestItem
    {
        public int Delay { get; set; }
        public override TestItemProcessResultEnum Process(TestRunContext ctx)
        {
            Thread.Sleep(Delay);
            return TestItemProcessResultEnum.Success;
        }

        public override string ToString()
        {
            return $"delay ({Delay} ms)";
        }

        public override void ParseXml(IAutoTest set, XElement item)
        {
            Delay = int.Parse(item.Attribute("delay").Value);
            base.ParseXml(set, item);
        }

        public override string ToXml()
        {
            return $"<delay delay=\"{Delay}\"/>";
        }
    }
}