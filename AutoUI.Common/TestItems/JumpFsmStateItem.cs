using AutoUI.Common;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "jumpFsmState")]
    public class JumpFsmStateItem : AutoTestItem
    {
        public CodeSection Target { get; set; }
        public override TestItemProcessResultEnum Process(TestRunContext ctx)
        {
            ((SpawnableAutoTest)ctx.Test).CurrentCodeSection = Target;
            ctx.CodePointer = 0;
            
            return TestItemProcessResultEnum.Success;
        }

        public override void ParseXml(IAutoTest set, XElement item)
        {
            var tname = item.Attribute("targetName").Value;
            Target = ((SpawnableAutoTest)set).Sections.First(z => z.Name == tname);            
            base.ParseXml(set, item);
        }

        public override string ToXml()
        {
            return $"<jumpFsmState targetName=\"{Target.Name}\"/>";
        }
    }
}