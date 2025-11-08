using AutoUI.Common;
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
            if (CursorToFindItem)
            {
                var fi = ctx.Vars[FindItemVarName] as PatternFindInfo;
                Cursor.Position = new System.Drawing.Point(fi.Rect.Left + fi.Rect.Width / 2, fi.Rect.Top + fi.Rect.Height / 2);
            }
            else
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

        public override void ParseXml(AutoTest set, XElement item)
        {
            IsRelative = bool.Parse(item.Attribute("isRelative").Value);
            X = int.Parse(item.Attribute("x").Value);
            Y = int.Parse(item.Attribute("y").Value);

            if (item.Attribute("cursorToFindItem") != null)
                CursorToFindItem = bool.Parse(item.Attribute("cursorToFindItem").Value);

            if (item.Attribute("findItemVarName") != null)
                FindItemVarName = item.Attribute("findItemVarName").Value;

            base.ParseXml(set, item);
        }

        public bool CursorToFindItem { get; set; }
        public string FindItemVarName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsRelative { get; set; }

        public override string ToXml()
        {
            return $"<cursorPos isRelative=\"{IsRelative}\"  cursorToFindItem=\"{CursorToFindItem}\" findItemVarName=\"{FindItemVarName}\" x=\"{X}\" y=\"{Y}\"/>";
        }
    }
}
