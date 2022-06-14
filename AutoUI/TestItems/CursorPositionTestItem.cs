using System.Windows.Forms;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "cursorPos")]
    public class CursorPositionTestItem : AutoTestItem
    {
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            var pos = Cursor.Position;
            if (IsRelative)
            {
                Cursor.Position = new System.Drawing.Point(pos.X + X, pos.Y + Y);
            }
            else
            {
                Cursor.Position = new System.Drawing.Point(X, Y);
            }
            return TestItemProcessResultEnum.Success;
        }

        public override void ParseXml(TestSet set, XElement item)
        {
            IsRelative = bool.Parse(item.Attribute("isRelative").Value);
            X = int.Parse(item.Attribute("x").Value);
            Y = int.Parse(item.Attribute("y").Value);
            base.ParseXml(set, item);
        }

        public int X { get; set; }
        public int Y { get; set; }
        public bool IsRelative { get; set; }
        internal override string ToXml()
        {
            return $"<cursorPos isRelative=\"{IsRelative}\" x=\"{X}\" y=\"{Y}\"/>";
        }
    }
}
