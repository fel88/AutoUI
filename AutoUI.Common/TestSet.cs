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

            foreach (var titem in root.Elements())
            {
                if (titem.Name.LocalName == "test")                
                    Tests.Add(new AutoTest(this, titem));
                
                if (titem.Name.LocalName == "spawnableTest")                
                    Tests.Add(new SpawnableAutoTest(this, titem));
            }
        }

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
            sb.AppendLine("</set>");
            return XElement.Parse(sb.ToString());
        }

    }
}
