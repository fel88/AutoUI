using System.Xml.Linq;

namespace AutoUI
{
    public abstract class AutoTestItem
    {
        public virtual void Init() { }
        public abstract void Process(AutoTestRunContext ctx);
        public virtual void ParseXml(XElement item) { }
        internal abstract string ToXml();
    }
}