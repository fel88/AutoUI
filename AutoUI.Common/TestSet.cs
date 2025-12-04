using System.IO.Compression;
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

            var scriptsNode = root.Element("scripts");
            if (scriptsNode != null)
                foreach (var item in scriptsNode.Elements("script"))
                {
                    switch (item.Attribute("type").Value)
                    {
                        case nameof(StartupScript):
                            StartupScript = item.Value;
                            break;
                        case nameof(FinalizerScript):
                            FinalizerScript = item.Value;
                            break;
                        case nameof(BeforeTestScript):
                            BeforeTestScript = item.Value;
                            break;
                        case nameof(AfterTestScript):
                            AfterTestScript = item.Value;
                            break;
                    }
                }

            var resourcesNode = root.Element("resources");
            if (resourcesNode != null)
                foreach (var item in resourcesNode.Elements("resource"))
                {
                    var type = item.Attribute("type").Value;

                    AutoTestResource resource = null;
                    if (type == "text")
                        resource = new TextAutoTestResource(item);

                    if (resource == null)
                        continue;

                    Resources.Add(resource);
                }

            var varsNode = root.Element("vars");
            if (varsNode != null)
                foreach (var item in varsNode.Elements("var"))
                {
                    Vars.Add(item.Attribute("key").Value, item.Value);
                }
        }

        public List<AutoTestResource> Resources = new List<AutoTestResource>();

        public string StartupScript = DefaultScripts.DefaultTestSetScript;
        public string FinalizerScript = DefaultScripts.DefaultTestSetScript;
        public string BeforeTestScript = DefaultScripts.DefaultTestScript;
        public string AfterTestScript = DefaultScripts.DefaultTestScript;

        public string Name { get; set; }
        public string ProcessPath;
        public List<IAutoTest> Tests = new List<IAutoTest>();
        public PatternMatchingPool Pool = new PatternMatchingPool();
        public Dictionary<string, string> Vars = new Dictionary<string, string>();

        public void ToStream(Stream stream)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<root>");
            var el1 = ToXml();
            sb.AppendLine(el1.ToString());

            sb.AppendLine("</root>");
            sb.ToString();

            var doc = XDocument.Parse(sb.ToString());

            using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                var entry1 = zipArchive.CreateEntry("tests.axml");
                using (var entryStream = entry1.Open())
                {
                    doc.Save(entryStream);
                }
                foreach (var item in Resources.Where(z => z.ResourceLoadType != ResourceLoadTypeEnum.Internal))
                {
                    var entry2 = zipArchive.CreateEntry(item.Path);
                    using (var entryStream = entry2.Open())
                    {
                        item.StoreData(entryStream);
                    }
                }
            }
        }

        public XElement ToXml()
        {
            StringBuilder sb = new();
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

            sb.AppendLine("<scripts>");
            sb.AppendLine(new XElement("script", new XAttribute("type", nameof(StartupScript)), new XCData(StartupScript)).ToString());
            sb.AppendLine(new XElement("script", new XAttribute("type", nameof(FinalizerScript)), new XCData(FinalizerScript)).ToString());
            sb.AppendLine(new XElement("script", new XAttribute("type", nameof(BeforeTestScript)), new XCData(BeforeTestScript)).ToString());
            sb.AppendLine(new XElement("script", new XAttribute("type", nameof(AfterTestScript)), new XCData(AfterTestScript)).ToString());
            sb.AppendLine("</scripts>");

            sb.AppendLine("<vars>");
            foreach (var var in Vars)
            {
                XElement el = new XElement("var", new XCData(var.Value));
                el.Add(new XAttribute("key", var.Key));
                sb.AppendLine(el.ToString());
            }

            sb.AppendLine("</vars>");
            sb.AppendLine("</set>");
            return XElement.Parse(sb.ToString());
        }

        public static TestSet LoadFromAZip(string path)
        {
            using ZipArchive archive = ZipFile.OpenRead(path);
            return LoadFromZipArchive(archive);
        }

        public static TestSet LoadFromAZipStream(Stream stream)
        {
            using ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read);
            return LoadFromZipArchive(archive);
        }

        public static TestSet LoadFromZipArchive(ZipArchive archive)
        {
            TestSet set = null;
            // Open the ZIP archive for reading

            // Filter entries that are at the root level (no path separator in FullName)
            var rootEntries = archive.Entries
                .Where(entry => !entry.FullName.Contains(Path.DirectorySeparatorChar) &&
                                !entry.FullName.Contains(Path.AltDirectorySeparatorChar) &&
                                !string.IsNullOrEmpty(entry.Name));

            foreach (ZipArchiveEntry entry in rootEntries)
            {
                if (entry.FullName.EndsWith(".axml"))
                {
                    using (Stream entryStream = entry.Open())
                    {
                        var doc = XDocument.Load(entryStream, LoadOptions.None);
                        set = new TestSet(doc.Root.Element("set"));

                    }
                }
            }

            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                var fr = set.Resources.FirstOrDefault(z => z.ResourceLoadType == ResourceLoadTypeEnum.External && z.Path == entry.FullName);
                if (fr == null)
                    continue;

                using (Stream entryStream = entry.Open())
                {
                    fr.LoadData(entryStream);
                }
            }
            return set;
        }
    }
}
