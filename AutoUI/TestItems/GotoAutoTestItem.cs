using System;
using System.Linq;

namespace AutoUI.TestItems
{
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

        internal override string ToXml()
        {
            throw new NotImplementedException();
        }
    }
}