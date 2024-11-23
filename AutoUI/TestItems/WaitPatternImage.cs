using AutoUI.TestItems.Editors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [TestItemEditor(Editor = typeof(WaitPatternEditor))]
    [XmlParse(XmlKey = "waitPattern")]
    public class WaitPatternImage : AutoTestItem
    {
        public List<PatternMatchingImage> Patterns = new List<PatternMatchingImage>();


        public override void ParseXml(TestSet parent, XElement item)
        {
            var pIds = item.Attribute("patternIds").Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            if (item.Attribute("moveCursor") != null)
                MoveCursorOnSuccessed = bool.Parse(item.Attribute("moveCursor").Value);
            if (item.Attribute("timeout") != null)
                Timeout = int.Parse(item.Attribute("timeout").Value);

            var pns = pIds.Select(zz => parent.Pool.Patterns.First(z => z.Id == zz)).ToList();
            Patterns = pns;
            base.ParseXml(parent, item);
        }

        public bool MoveCursorOnSuccessed { get; set; } = false;
        public int Timeout { get; set; } = 100 * 1000;
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            Stopwatch start = Stopwatch.StartNew();
            Point? ret = null;
            if (Patterns.Count == 0)
                return TestItemProcessResultEnum.Success;

            while (true)
            {
                if (start.Elapsed.TotalMilliseconds > Timeout)
                {
                    return TestItemProcessResultEnum.Failed;
                }
                var screen = SearchByPatternImage.GetScreenshot();
                Bitmap target = null;
                foreach (var pattern in Patterns)
                {
                    foreach (var item in pattern.Items)
                    {
                        ret = SearchByPatternImage.SearchPattern(screen, item);
                        if (ret != null)
                        {
                            target = item.Bitmap;
                            break;
                        }
                    }
                    if (ret != null)
                    {
                        if (MoveCursorOnSuccessed)
                        {
                            SearchByPatternImage.SetCursorPos(ret.Value.X + target.Width / 2, ret.Value.Y + target.Height / 2);
                        }
                        return TestItemProcessResultEnum.Success;
                    }
                }
                screen.Dispose();
                ctx.LastSearchPosition = ret;
            }
        }

        internal override string ToXml()
        {
            return $"<waitPattern patternIds=\"{string.Join(";", Patterns.Select(z => z.Id).ToArray())}\" timeout=\"{Timeout}\"  moveCursor=\"{MoveCursorOnSuccessed}\" ></waitPattern>";
        }

        public bool Assert { get; set; }
    }
}