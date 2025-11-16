using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AutoUI.Common
{
    public abstract class AutoTestItem
    {
        public AutoTestItem()
        {
            Id = Helpers.GetNewId();
        }
        public int Id { get; private set; }
        public abstract TestItemProcessResultEnum Process(AutoTestRunContext ctx);
        public virtual void ParseXml(IAutoTest parent, XElement item) { }
        public abstract string ToXml();

        public virtual AutoTestItem Clone()
        {
            throw new NotImplementedException();
        }

        public AutoTestItem Parent;
        public IAutoTest ParentTest;//?
        public List<AutoTestItem> Childs = new List<AutoTestItem>();
    }
}