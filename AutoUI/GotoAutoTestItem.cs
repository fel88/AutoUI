using System;
using System.Linq;

namespace AutoUI
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

        public override void Process(AutoTestRunContext ctx)
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
                var fr = ctx.Test.Items.OfType<LabelAutoTestItem>().First(z => z.Label == Label);
                ctx.CodePointer = ctx.Test.Items.IndexOf(fr);
                ctx.ForceCodePointer = true;
            }

        }

        internal override string ToXml()
        {
            throw new NotImplementedException();
        }
    }
}