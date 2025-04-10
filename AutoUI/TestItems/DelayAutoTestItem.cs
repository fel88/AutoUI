﻿using System.CodeDom;
using System.Threading;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "delay")]
    public class DelayAutoTestItem : AutoTestItem
    {
        public int Delay { get; set; }
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            Thread.Sleep(Delay);
            return TestItemProcessResultEnum.Success;
        }

        public override void ParseXml(AutoTest set, XElement item)
        {
            Delay = int.Parse(item.Attribute("delay").Value);
            base.ParseXml(set, item);
        }

        internal override string ToXml()
        {
            return $"<delay delay=\"{Delay}\"/>";
        }
    }
}