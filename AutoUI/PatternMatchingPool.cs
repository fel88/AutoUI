using AutoUI.TestItems;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AutoUI
{
    public class PatternMatchingPool
    {
        public List<PatternMatchingImage> Patterns = new List<PatternMatchingImage>();

        internal void ParseXml(TestSet set, XElement xElement)
        {
            foreach (var xx in xElement.Elements("pattern"))
            {
                var tp = xx.Attribute("type").Value;
                if (tp != "image") continue;
                var nn = new PatternMatchingImage();
                Patterns.Add(nn);
                nn.ParseXml(xx);
            }
        }

        internal void ToXml(StringBuilder sb)
        {
            sb.AppendLine("<pool>");
            foreach (var item in Patterns)
            {
                item.ToXml(sb);
            }
            sb.AppendLine("</pool>");
        }
    }
}
