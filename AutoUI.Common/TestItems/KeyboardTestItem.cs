using AutoUI.Common;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "keyboard")]
    public class KeyboardTestItem : AutoTestItem
    {
        public string Command { get; set; } = "^{c}";        
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            SendKeys.SendWait(Command);
            SendKeys.Flush();
            return TestItemProcessResultEnum.Success;
        }

        public override string ToString()
        {
            return $"keyboard ({Command})";
        }

        public override void ParseXml(IAutoTest set, XElement item)
        {
            Command = item.Value;
            
            base.ParseXml(set, item);
        }

        public override string ToXml()
        {
            return $"<keyboard name=\"{Name}\" >{new XCData(Command)}</keyboard>";
        }
    }
}
