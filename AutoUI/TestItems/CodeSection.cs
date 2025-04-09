using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    public class CodeSection
    {
        public string Name { get; set; }
        public List<AutoTestItem> Items = new List<AutoTestItem>();
        public CodeSectionRole Role { get; set; }
        internal CodeSection Clone()
        {
            CodeSection ret = new CodeSection();
            ret.Name = Name;
            ret.Role = Role;
            foreach (var item in Items)
            {
                ret.Items.Add(item.Clone());
            }
            return ret;
        }

        internal void ParseXml(AutoTest test, XElement section)
        {
            Name = section.Attribute("name").Value;
            if (section.Attribute("role") != null)
                Role = (CodeSectionRole)Enum.Parse(typeof(CodeSectionRole), section.Attribute("role").Value);

            Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(z => z.GetCustomAttribute(typeof(XmlParseAttribute)) != null).ToArray();

            foreach (var item in section.Elements())
            {

                var fr = types.FirstOrDefault(z => (z.GetCustomAttribute(typeof(XmlParseAttribute)) as XmlParseAttribute).XmlKey == item.Name);
                if (fr != null)
                {
                    var tp = Activator.CreateInstance(fr) as AutoTestItem;
                    tp.ParseXml(test, item);
                    tp.ParentTest = test;
                    Items.Add(tp);

                }
            }
        }

        internal void ToXml(StringBuilder sb)
        {
            sb.AppendLine($"<section name=\"{Name}\" role=\"{Role}\">");
            foreach (var item in Items)
            {
                sb.AppendLine(item.ToXml());
            }
            sb.AppendLine("</section>");
        }
    }
}
