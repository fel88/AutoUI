using System.Drawing;

namespace AutoUI.TestItems
{
    public class PatternMatchingImageItem
    {
        public int Id;
        public string Name { get; set; } = "pm image item";
        public Bitmap Bitmap;
        public PatternMatchingMode Mode { get; set; }

    }
}