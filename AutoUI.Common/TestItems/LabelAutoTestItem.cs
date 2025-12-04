using AutoUI.Common;
using System;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "label")]
    public class LabelAutoTestItem : AutoTestItem
    {
        public string Label { get; set; }
        public override TestItemProcessResultEnum Process(TestRunContext ctx)
        {
            return TestItemProcessResultEnum.Success;
        }

        public override string ToString()
        {
            return $"label ({Label})";
        }

        public override void ParseXml(IAutoTest set, XElement item)
        {
            if (item.Attribute("label") != null)            
                Label = item.Attribute("label").Value;
                        
            base.ParseXml(set, item);
        }

        public override string ToXml()
        {
            return $"<label label=\"{Label}\"/>";
        }
    }
}
