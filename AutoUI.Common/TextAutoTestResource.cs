using System.Text;
using System.Xml.Linq;

namespace AutoUI.Common
{
    public class TextAutoTestResource : AutoTestResource
    {
        public string Text;

        public TextAutoTestResource()
        {
        }

        public TextAutoTestResource(XElement item)
        {
            Name = item.Attribute("name").Value;
            Path = item.Element("path").Value;
            ResourceLoadType = Enum.Parse<ResourceLoadTypeEnum>(item.Attribute("location").Value);
            if (item.Attribute("internalStorageMode") != null)
                InternalStorageMode = Enum.Parse<InternalResourceStorageModeEnum>(item.Attribute("internalStorageMode").Value);

            if (ResourceLoadType == ResourceLoadTypeEnum.Internal)
            {
                if (InternalStorageMode == InternalResourceStorageModeEnum.Text)
                    Text = item.Element("data").Value;
                else
                    Text = Encoding.UTF8.GetString(Convert.FromBase64String(item.Element("data").Value));
            }
        }
        public InternalResourceStorageModeEnum InternalStorageMode { get; set; }
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
            ret.Add(new XAttribute("internalStorageMode", InternalStorageMode));
            ret.Add(new XElement("path", new XCData(Path)));

            if (ResourceLoadType == ResourceLoadTypeEnum.Internal)
            {
                if (InternalStorageMode == InternalResourceStorageModeEnum.Text)
                    ret.Add(new XElement("data", new XCData(Text)));
                else
                    ret.Add(new XElement("data", new XCData(Convert.ToBase64String(Encoding.UTF8.GetBytes(Text)))));
            }

            return ret;
        }
    }
}
