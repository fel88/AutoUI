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
        /// <summary>
        /// percentage of acceptable error level
        /// </summary>
        public double PixelsMatchAcceptableErrorLevel { get; set; } = 0;

        public void ToXml(StringBuilder sb)
        {
            sb.AppendLine($"<item name=\"{Name}\" mode=\"{Mode}\" pixelsMode=\"{PixelsMode}\" " +
                $"pixelDist=\"{PixelsMatchDistancePerChannel}\" " +
                $"pixelErrorRate=\"{PixelsMatchAcceptableErrorLevel}\"" +
                $" >");
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
                PixelsMatchDistancePerChannel = item.Attribute("pixelDist").Value.ToDouble();
            
            if (item.Attribute("pixelErrorRate") != null)
                PixelsMatchAcceptableErrorLevel = item.Attribute("pixelErrorRate").Value.ToDouble();

        }
    }
}