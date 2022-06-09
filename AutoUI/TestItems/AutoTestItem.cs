using System.Collections.Generic;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    public abstract class AutoTestItem
    {
        public AutoTestItem()
        {
            Id = Helpers.GetNewId();
        }
        public int Id { get; private set; }
        public virtual void Init() { }
        public abstract TestItemProcessResultEnum Process(AutoTestRunContext ctx);
        public virtual void ParseXml(TestSet parent, XElement item) {        }
        internal abstract string ToXml();

        public AutoTestItem Parent;
        public List<AutoTestItem> Childs = new List<AutoTestItem>();
    }

    public enum TestItemProcessResultEnum
    {
        Success, Failed
    }
}