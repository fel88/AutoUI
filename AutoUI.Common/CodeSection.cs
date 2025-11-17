using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.Common
{
    public class CodeSection
    {
        public CodeSection() { }
        public CodeSection(IAutoTest test, XElement section)
        {
            Name = section.Attribute("name").Value;
            if (section.Attribute("role") != null)
                Role = Enum.Parse<CodeSectionRole>(section.Attribute("role").Value);

            var types = Assembly.GetExecutingAssembly().GetTypes().Where(z => z.GetCustomAttribute(typeof(XmlParseAttribute)) != null).ToArray();

            foreach (var item in section.Elements())
            {
                var fr = types.FirstOrDefault(z => (z.GetCustomAttribute(typeof(XmlParseAttribute)) as XmlParseAttribute).XmlKey == item.Name);
                if (fr == null)
                    continue;

                var tp = Activator.CreateInstance(fr) as AutoTestItem;
                tp.ParseXml(test, item);
                tp.ParentTest = test;
                Items.Add(tp);
            }
        }
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



        public XElement ToXml()
        {
            XElement element = new XElement("section");
            element.Add(new XAttribute("name", Name ?? string.Empty));
            element.Add(new XAttribute("role", Role));

            foreach (var item in Items)
            {
                element.Add(XElement.Parse(item.ToXml()));
            }
            return element;
        }
    }
}
