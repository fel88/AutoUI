using System;

namespace AutoUI
{
    public class LabelAutoTestItem : AutoTestItem
    {

        public string Label { get; set; }
        public override void Process(AutoTestRunContext ctx)
        {

        }

        internal override string ToXml()
        {
            throw new NotImplementedException();
        }
    }
}
