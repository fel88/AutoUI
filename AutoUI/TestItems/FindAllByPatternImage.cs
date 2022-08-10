using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    //[TestItemEditor(Editor = typeof(SearchByPatternEditor))]
    [XmlParse(XmlKey = "findAll")]
    public class FindAllByPatternImage : AutoTestItem
    {
        public PatternMatchingImage Pattern;

        public PatternFindInfo[] Run()
        {
            List<PatternFindInfo> infos = new List<PatternFindInfo>();
            var screen = SearchByPatternImage.GetScreenshot();
            infos.Clear();
            var sw = Stopwatch.StartNew();
            foreach (var item in Pattern.Items)
            {
                Point? res = null;
                do
                {
                    res = SearchByPatternImage.SearchPattern(screen, item, res == null ? 0 : (res.Value.X), res == null ? 0 : (res.Value.Y + 1));
                    if (res != null)
                        infos.Add(new PatternFindInfo()
                        {
                            Pattern = item,
                            Rect = new Rectangle(res.Value.X, res.Value.Y, item.Bitmap.Width, item.Bitmap.Height)
                        });

                } while (res != null);
            }

            //non-max suppress

            var ord = infos.OrderByDescending(z => z.Rect.Width * z.Rect.Height).ToArray();
            List<PatternFindInfo> todel = new List<PatternFindInfo>();
            for (int i = 0; i < ord.Length; i++)
            {
                for (int j = i + 1; j < ord.Length; j++)
                {
                    if (ord[i].Rect.Contains(ord[j].Rect))
                    {
                        todel.Add(ord[j]);
                    }
                }
            }

            foreach (var item in todel)
            {
                infos.Remove(item);
            }

            infos = infos.OrderBy(z => z.Rect.Left).ThenBy(z => z.Rect.Top).ToList();
            sw.Stop();
            return infos.ToArray();
        }

        public string StoreVarName { get; set; }
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            var f = Run();
            ctx.Vars[StoreVarName] = f.ToList();
            return TestItemProcessResultEnum.Success;

        }
        public override void ParseXml(TestSet set, XElement item)
        {
            if (item.Attribute("storeVarName") != null)
                StoreVarName = item.Attribute("storeVarName").Value;

            var pId = int.Parse(item.Attribute("patternId").Value);
            var p = set.Pool.Patterns.First(z => z.Id == pId);
            Pattern = p;

            base.ParseXml(set, item);
        }

        internal override string ToXml()
        {
            return $"<findAll patternId=\"{Pattern.Id}\" storeVarName=\"{StoreVarName}\" ></findAll>";

        }
    }
}