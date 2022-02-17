using System.Xml.Linq;

namespace AutoUI.TestItems
{
    public abstract class AutoTestItem
    {
        public virtual void Init() { }
        public abstract TestItemProcessResultEnum Process(AutoTestRunContext ctx);
        public virtual void ParseXml(XElement item) { }
        internal abstract string ToXml();
    }

    public enum TestItemProcessResultEnum
    {
        Success, Failed
    }
}