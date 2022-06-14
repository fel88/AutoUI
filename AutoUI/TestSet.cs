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

                sb.AppendLine("<section name=\"main\">");
                foreach (var item in test.Main.Items)
                {
                    sb.AppendLine(item.ToXml());
                }
                sb.AppendLine("</section>");
                sb.AppendLine("<section name=\"finalizer\">");
                foreach (var item in test.Finalizer.Items)
                {
                    sb.AppendLine(item.ToXml());
                }
                sb.AppendLine("</section>");
                sb.AppendLine("<section name=\"emitter\">");
                foreach (var item in test.Emitter.Items)
                {
                    sb.AppendLine(item.ToXml());
                }
                sb.AppendLine("</section>");
                sb.AppendLine("</test>");
            }

            Pool.ToXml(sb);
        }

        internal void ParseXml(XElement root)
        {
            foreach (var titem in root.Descendants("test"))
            {
                var test = new AutoTest(this);

                if (titem.Attribute("useEmitter") != null)
                    test.UseEmitter = bool.Parse(titem.Attribute("useEmitter").Value);

                test.ParseXml(titem);

                Tests.Add(test);
                //get all types
                Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(z => z.GetCustomAttribute(typeof(XmlParseAttribute)) != null).ToArray();
                foreach (var section in titem.Elements("section"))
                {
                    var sname = section.Attribute("name").Value;
                    foreach (var item in section.Elements())
                    {

                        var fr = types.FirstOrDefault(z => (z.GetCustomAttribute(typeof(XmlParseAttribute)) as XmlParseAttribute).XmlKey == item.Name);
                        if (fr != null)
                        {
                            var tp = Activator.CreateInstance(fr) as AutoTestItem;
                            tp.ParseXml(this, item);
                            if (sname == "main")
                            {
                                test.Main.Items.Add(tp);
                            }
                            if (sname == "finalizer")
                            {
                                test.Finalizer.Items.Add(tp);
                            }
                            if (sname == "emitter")
                            {
                                test.Emitter.Items.Add(tp);
                            }
                        }
                    }
                }

            }
        }
    }
}
