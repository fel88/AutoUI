using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    public class PatternMatchingImage
    {
        public PatternMatchingImage()
        {
            Id = Helpers.GetNewId();
        }
        public string Name { get; set; } = "patternMatchingImage";
        public int Id { get; private set; }
        public List<PatternMatchingImageItem> Items = new List<PatternMatchingImageItem>();


        internal void ToXml(StringBuilder sb)
        {
            sb.AppendLine($"<pattern type=\"image\" id=\"{Id}\" name=\"{Name}\">");
            foreach (var item in Items)
            {
                sb.AppendLine($"<item name=\"{item.Name}\" mode=\"{item.Mode}\">");
                MemoryStream ms = new MemoryStream();
                item.Bitmap.Save(ms, ImageFormat.Png);
                var bb = Convert.ToBase64String(ms.ToArray());
                sb.AppendLine(bb); sb.AppendLine("</item>");
            }
            sb.AppendLine("</pattern>");
        }

        internal void ParseXml(XElement xx)
        {
            Name = xx.Attribute("name").Value;

            Id = int.Parse(xx.Attribute("id").Value);
            foreach (var item in xx.Elements("item"))
            {
                var data = Convert.FromBase64String(item.Value);
                MemoryStream ms = new MemoryStream(data);
                var bmp = Bitmap.FromStream(ms) as Bitmap;
                Items.Add(new PatternMatchingImageItem() { Bitmap = bmp });
                if (item.Attribute("mode") != null)
                    Items.Last().Mode = (PatternMatchingMode)(Enum.Parse(typeof(PatternMatchingMode), item.Attribute("mode").Value, true));
                if (item.Attribute("name") != null)
                    Items.Last().Name = item.Attribute("mode").Value;
            }
        }
    }

    public class PatternMatchingImageItem
    {
        public int Id;
        public string Name { get; set; } = "pm image item";
        public Bitmap Bitmap;
        public PatternMatchingMode Mode { get; set; }

    }

    public enum PatternMatchingMode
    {
        Precise,
        Grayscale,
        BinaryMean//binary mean>128
    }
}