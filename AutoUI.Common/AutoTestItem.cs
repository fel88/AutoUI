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
        public abstract TestItemProcessResultEnum Process(TestRunContext ctx);
        public virtual void ParseXml(IAutoTest parent, XElement item)
        {
            Parent = parent;
            if (item.Attribute("name") != null)
                Name = item.Attribute("name").Value;
        }
        public abstract string ToXml();

        public virtual AutoTestItem Clone()
        {
            var ret = Activator.CreateInstance(this.GetType()) as AutoTestItem;
            ret.Parent = Parent;
            ret.ParseXml(Parent, XElement.Parse(ToXml()));
            return ret;
        }
        
        
        public IAutoTest Parent;//?
        public List<AutoTestItem> Childs = new List<AutoTestItem>();
    }
}