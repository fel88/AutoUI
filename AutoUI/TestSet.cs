using AutoUI.TestItems;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace AutoUI
{
    public class TestSet
    {
        public string Name { get; set; }
        public string ProcessPath;
        public List<AutoTest> Tests = new List<AutoTest>();
        public PatternMatchingPool Pool = new PatternMatchingPool();

        internal void AppendXml(StringBuilder sb)
        {
            foreach (var test in Tests)
            {
                sb.AppendLine($"<test id=\"{test.Id}\" name=\"{test.Name}\" useEmitter=\"{test.UseEmitter}\">");

                sb.AppendLine("<vars>");
                foreach (var item in test.Data)
                {
                    sb.AppendLine($"<item key=\"{item.Key}\" value=\"{item.Value}\"/>");
                }
                sb.AppendLine("</vars>");

                foreach (var item in test.Sections)                        
                    item.ToXml(sb);                
               
                sb.AppendLine("</test>");
            }

            Pool.ToXml(sb);
        }

        internal void ParseXml(XElement root)
        {
            foreach (var titem in root.Descendants("test"))
            {
                var test = new AutoTest(this);

                if (titem.Element("vars") != null)
                {
                    foreach (var kitem in titem.Element("vars").Elements("item"))
                    {
                        test.Data.Add(kitem.Attribute("key").Value, kitem.Attribute("value").Value);
                    }
                }
                if (titem.Attribute("useEmitter") != null)
                    test.UseEmitter = bool.Parse(titem.Attribute("useEmitter").Value);

                test.ParseXml(titem);

                Tests.Add(test);
                //get all types
                foreach (var section in titem.Elements("section"))
                {
                    CodeSection _section = new CodeSection();
                    _section.ParseXml(test, section);                    
                    test.Sections.Add(_section);                   
                }
            }
        }
    }
}
