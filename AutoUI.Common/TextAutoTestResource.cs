
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.Common
{
    public class TextAutoTestResource : AutoTestResource
    {
        public string Text;

        public override void LoadData(Stream stream)
        {
            using var reader = new StreamReader(stream, Encoding.UTF8);
            Text = reader.ReadToEnd();
        }

        public override void StoreData(Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.Write(Text);
                writer.Flush(); // Ensure all data is written from the buffer to the underlying stream
            }
        }

        public override XElement ToXml()
        {
            XElement ret = new XElement("resource");
            ret.Add(new XAttribute("name", Name));
            ret.Add(new XAttribute("type", "text"));
            ret.Add(new XAttribute("location", ResourceLoadType));
            ret.Add(new XElement("path", new XCData(Path)));
            return ret;
        }
    }
}
