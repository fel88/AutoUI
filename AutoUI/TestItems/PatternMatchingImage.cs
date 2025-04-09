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
                item.ToXml(sb);                
            
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
                var restoreItem = new PatternMatchingImageItem() { Bitmap = bmp };
                restoreItem.ParseXml(item);
                Items.Add(restoreItem);
                
            }
        }
    }
}