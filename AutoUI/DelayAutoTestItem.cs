using System.Threading;

namespace AutoUI
{
    public class DelayAutoTestItem : AutoTestItem
    {
        public int Delay { get; set; }
        public override void Process(AutoTestRunContext ctx)
        {
            Thread.Sleep(Delay);
        }

        internal override string ToXml()
        {
            return $"<delay delay=\"{Delay}\"/>";
        }
    }
}