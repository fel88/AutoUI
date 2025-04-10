﻿using System.Linq;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "goto")]
    public class GotoAutoTestItem : AutoTestItem
    {
        public override void Init()
        {
            Counter = CounterInitValue;
            base.Init();
        }
        public int CounterInitValue { get; set; } = 0;
        public int Counter = 0;
        public bool UseCounter { get; set; }
        public string Label { get; set; }

        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            bool skip = false;

            if (UseCounter)
            {
                Counter--;
                if (Counter <= 0)
                {
                    skip = true;
                }
            }
            if (!skip)
            {
                var fr = ctx.Test.Main.Items.OfType<LabelAutoTestItem>().First(z => z.Label == Label);
                ctx.CodePointer = ctx.Test.Main.Items.IndexOf(fr);
                ctx.ForceCodePointer = true;
            }

            return TestItemProcessResultEnum.Success;

        }

        public override void ParseXml(AutoTest set, XElement item)
        {
            Label = (item.Attribute("label").Value);

            if (item.Attribute("useCounter") != null)
                UseCounter = bool.Parse(item.Attribute("useCounter").Value);

            if (item.Attribute("iterations") != null)
                CounterInitValue = int.Parse(item.Attribute("iterations").Value);

            base.ParseXml(set, item);
        }

        internal override string ToXml()
        {
            return $"<goto label=\"{Label}\" useCounter=\"{UseCounter}\" iterations=\"{CounterInitValue}\"/>";
        }
    }
}