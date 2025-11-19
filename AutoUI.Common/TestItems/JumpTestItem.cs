using AutoUI.Common;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "jump")]
    public class JumpTestItem : AutoTestItem
    {
        public string JumpLabel { get; set; }

        public override string ToString()
        {
            return $"jump ({JumpLabel})";
        }

        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            if (ctx.LastSearchPosition != null)
            {
                var fr = ctx.Test.CurrentCodeSection.Items.OfType<LabelAutoTestItem>().First(z => z.Label == JumpLabel);
                ctx.CodePointer = ctx.Test.CurrentCodeSection.Items.IndexOf(fr);
                ctx.ForceCodePointer = true;
            }


            return TestItemProcessResultEnum.Success;

        }

        public override void ParseXml(IAutoTest set, XElement item)
        {

            JumpLabel = (item.Attribute("label").Value);



            base.ParseXml(set, item);
        }

        public override string ToXml()
        {
            return $"<jump  label=\"{JumpLabel}\"  />";
        }
    }
}