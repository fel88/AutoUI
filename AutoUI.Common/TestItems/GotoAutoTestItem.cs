using AutoUI.Common;
using System.Linq;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "goto")]
    public class GotoAutoTestItem : AutoTestItem
    {

        public int CounterInitValue { get; set; } = 0;

        public bool UseCounter { get; set; }
        public string Label { get; set; }

        public class GotoTestStepContext
        {
            public GotoTestStepContext(int CounterInitValue)
            {
                Counter = CounterInitValue;
            }

            public int Counter = 0;

            internal bool Step()
            {
                bool skip = false;

                Counter--;
                if (Counter <= 0)
                {
                    skip = true;
                }
                return skip;
            }
        }

        public override TestItemProcessResultEnum Process(TestRunContext ctx)
        {
            GotoTestStepContext stepCtx = null;
            if (!ctx.TagRegisters.ContainsKey(this))
            {
                ctx.TagRegisters.Add(this, new GotoTestStepContext(CounterInitValue));
            }
            stepCtx = ctx.TagRegisters[this] as GotoTestStepContext;

            bool skip = false;
            if (UseCounter)
                skip = stepCtx.Step();

            if (!skip)
            {
                var fr = ctx.Test.CurrentCodeSection.Items.OfType<LabelAutoTestItem>().First(z => z.Label == Label);
                ctx.CodePointer = ctx.Test.CurrentCodeSection.Items.IndexOf(fr);
                ctx.ForceCodePointer = true;
            }

            return TestItemProcessResultEnum.Success;

        }

        public override void ParseXml(IAutoTest set, XElement item)
        {
            Label = (item.Attribute("label").Value);

            if (item.Attribute("useCounter") != null)
                UseCounter = bool.Parse(item.Attribute("useCounter").Value);

            if (item.Attribute("iterations") != null)
                CounterInitValue = int.Parse(item.Attribute("iterations").Value);

            base.ParseXml(set, item);
        }

        public override string ToString()
        {
            return $"goto ({Label})";
        }

        public override string ToXml()
        {
            return $"<goto label=\"{Label}\" useCounter=\"{UseCounter}\" iterations=\"{CounterInitValue}\"/>";
        }
    }
}