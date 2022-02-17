using System;

namespace AutoUI.TestItems
{
    public class LabelAutoTestItem : AutoTestItem
    {

        public string Label { get; set; }
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            return TestItemProcessResultEnum.Success;
        }

        internal override string ToXml()
        {
            throw new NotImplementedException();
        }
    }
}
