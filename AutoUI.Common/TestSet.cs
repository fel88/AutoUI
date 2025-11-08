using System.Text;
using System.Xml.Linq;

namespace AutoUI.Common
{
    public class TestSet
    {
        public TestSet() { }
        public TestSet(XElement root)
        {
            if (root.Element("pool") != null)
                Pool.ParseXml(this, root.Element("pool"));

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
                    CodeSection _section = new CodeSection(test, section);
                    test.Sections.Add(_section);
                }
            }
        }
        public string Name { get; set; }
        public string ProcessPath;
        public List<AutoTest> Tests = new List<AutoTest>();
        public PatternMatchingPool Pool = new PatternMatchingPool();

        public XElement ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<set>");
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
                    sb.Append(item.ToXml());

                sb.AppendLine("</test>");
            }

            Pool.ToXml(sb);
            sb.AppendLine("</set>");
            return XElement.Parse(sb.ToString());
        }

    }
}
