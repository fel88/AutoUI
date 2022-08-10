using System;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "label")]
    public class LabelAutoTestItem : AutoTestItem
    {
        public string Label { get; set; }
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            return TestItemProcessResultEnum.Success;
        }
        public override void ParseXml(TestSet set, XElement item)
        {
            if (item.Attribute("label") != null)            
                Label = item.Attribute("label").Value;
                        
            base.ParseXml(set, item);
        }

        internal override string ToXml()
        {
            return $"<label label=\"{Label}\"/>";
        }
    }
}
