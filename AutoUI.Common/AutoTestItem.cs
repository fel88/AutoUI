using System.Xml.Linq;

namespace AutoUI.Common
{
    public abstract class AutoTestItem
    {
        public AutoTestItem()
        {
            Id = Helpers.GetNewId();
            Name = $"{GetType().Name}_{Id}";
        }

        public int Id { get; private set; }
        public string Name { get; set; }
        public abstract TestItemProcessResultEnum Process(AutoTestRunContext ctx);
        public virtual void ParseXml(IAutoTest parent, XElement item)
        {
            if (item.Attribute("name") != null)
                Name = item.Attribute("name").Value;
        }
        public abstract string ToXml();

        public virtual AutoTestItem Clone()
        {
            var ret = Activator.CreateInstance(this.GetType()) as AutoTestItem;
            ret.ParseXml(ParentTest, XElement.Parse(ToXml()));
            return ret;
        }

        public AutoTestItem Parent;
        public IAutoTest ParentTest;//?
        public List<AutoTestItem> Childs = new List<AutoTestItem>();
    }
}