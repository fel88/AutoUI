using System.Text;
using System.Xml.Linq;

namespace AutoUI.Common
{
    public class TestSet
    {
        public TestSet() { }
        public TestSet(XElement root)
        {
            if (root.Elements().Count() == 1 && root.Element("set") != null)
                root = root.Element("set");

            if (root.Element("pool") != null)
                Pool.ParseXml(this, root.Element("pool"));

            foreach (var titem in root.Elements())
            {
                if (titem.Name.LocalName == "test")
                    Tests.Add(new AutoTest(this, titem));

                if (titem.Name.LocalName == "spawnableTest")
                    Tests.Add(new SpawnableAutoTest(this, titem));
            }

            var resourcesNode = root.Element("resources");
            if (resourcesNode == null)
                return;

            foreach (var item in resourcesNode.Elements("resource"))
            {
                var type = item.Attribute("type").Value;

                AutoTestResource resource = null;
                if (type == "text")
                    resource = new TextAutoTestResource();

                if (resource == null)
                    continue;

                resource.Name = item.Attribute("name").Value;
                resource.Path = item.Element("path").Value;
                resource.ResourceLoadType = Enum.Parse<ResourceLoadTypeEnum>(item.Attribute("location").Value);

                Resources.Add(resource);
            }
        }

        public List<AutoTestResource> Resources = new List<AutoTestResource>();

        public string Name { get; set; }
        public string ProcessPath;
        public List<IAutoTest> Tests = new List<IAutoTest>();
        public PatternMatchingPool Pool = new PatternMatchingPool();

        public XElement ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<set>");
            foreach (var test in Tests)
            {
                sb.AppendLine(test.ToXml().ToString());
            }

            Pool.ToXml(sb);
            sb.AppendLine("<resources>");
            foreach (var resource in Resources)
            {
                sb.AppendLine(resource.ToXml().ToString());
            }

            sb.AppendLine("</resources>");
            sb.AppendLine("</set>");
            return XElement.Parse(sb.ToString());
        }

    }
}
