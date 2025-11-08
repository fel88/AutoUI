using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.Common
{
    public class PatternMatchingImageItem
    {
        public int Id;
        public string Name { get; set; } = "pm image item";
        public Bitmap Bitmap;
        public PatternMatchingMode Mode { get; set; }
        public PixelsMatchingMode PixelsMode { get; set; }
        public double PixelsMatchDistancePerChannel { get; set; } = 15;

        public void ToXml(StringBuilder sb)
        {
            sb.AppendLine($"<item name=\"{Name}\" mode=\"{Mode}\" pixelsMode=\"{PixelsMode}\" pixelDist=\"{PixelsMatchDistancePerChannel}\" >");
            MemoryStream ms = new MemoryStream();
            Bitmap.Save(ms, ImageFormat.Png);
            var bb = Convert.ToBase64String(ms.ToArray());
            sb.AppendLine(bb);
            sb.AppendLine("</item>");
        }

        internal void ParseXml(XElement item)
        {
            if (item.Attribute("mode") != null)
                Mode = (PatternMatchingMode)(Enum.Parse(typeof(PatternMatchingMode), item.Attribute("mode").Value, true));

            if (item.Attribute("pixelsMode") != null)
                PixelsMode = (PixelsMatchingMode)(Enum.Parse(typeof(PixelsMatchingMode), item.Attribute("pixelsMode").Value, true));

            if (item.Attribute("name") != null)
                Name = item.Attribute("name").Value;

            if (item.Attribute("pixelDist") != null)
                PixelsMatchDistancePerChannel = double.Parse(item.Attribute("pixelDist").Value);

        }
    }
}