using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "wheel")]
    public class MouseWheelAutoTestItem : AutoTestItem
    {
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public int Delta { get; set; } = 120;
        public override void ParseXml(TestSet parent, XElement item)
        {
            base.ParseXml(parent, item);
        }
        internal override string ToXml()
        {
            return $"<wheel delta=\"{Delta}\"/>";
        }
    }
}
