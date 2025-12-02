using System.Text;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace AutoUI.Common
{
    public class SpawnableAutoTest: AbstractAutoTest, IAutoTest
    {
        public SpawnableAutoTest()
        {

        }

        
        public SpawnableAutoTest(TestSet parent) : this()
        {
            Parent = parent;
            Id = Helpers.GetNewId();
        }

        public SpawnableAutoTest(TestSet parent, XElement titem) : this()
        {
            Parent = parent;
            Id = int.Parse(titem.Attribute("id").Value);
            
            if (titem.Element("vars") != null)
            {
                foreach (var kitem in titem.Element("vars").Elements("item"))
                {
                    Data.Add(kitem.Attribute("key").Value, kitem.Attribute("value").Value);
                }
            }
            if (titem.Attribute("useEmitter") != null)
                UseEmitter = bool.Parse(titem.Attribute("useEmitter").Value);

            ParseXml(titem);

            
            //get all types
            foreach (var section in titem.Elements("section"))
            {
                CodeSection _section = new CodeSection(this, section);
                Sections.Add(_section);
            }
        }

        public TestSet Parent { get; private set; }
        public int Id { get; private set; }
        public string Name { get; set; }
        public int Delay = 0;

        public bool UseEmitter { get; set; } = false;

        public List<CodeSection> Sections = new List<CodeSection>();//fsm sections

        public CodeSection Finalizer => Sections.FirstOrDefault(z => z.Role == CodeSectionRole.Finalizer);
        public CodeSection Main => Sections.FirstOrDefault(z => z.Role == CodeSectionRole.Main);
        public CodeSection Emitter => Sections.FirstOrDefault(z => z.Role == CodeSectionRole.Emitter);

        public AutoTestRunContext lastContext;

        public CodeSection CurrentCodeSection = null;
        public AutoTestRunContext Run(AutoTestRunContext ctx = null)
        {
            CurrentCodeSection = Main;
            if (UseEmitter)
            {
                CurrentCodeSection = Emitter;
                ctx.State = TestStateEnum.Emitter;
            }

            if (ctx != null && ctx.IsSubTest)
                CurrentCodeSection = Main;

          


            if (ctx == null)
                ctx = new AutoTestRunContext(this);

            if (!ctx.IsSubTest)
            {
                //ctx.Test = this;
                lastContext = ctx;
            }

            foreach (var item in Data)
                ctx.Vars.Add(item.Key, item.Value);

            while (ctx.CodePointer < CurrentCodeSection.Items.Count && !ctx.Finished)
            {
                ctx.ForceCodePointer = false;
                var result = CurrentCodeSection.Items[ctx.CodePointer].Process(ctx);
                if (result == TestItemProcessResultEnum.Failed)
                {
                    if (FailedAction == TestFailedBehaviour.Terminate)
                        ctx.Finished = true;

                    ctx.WrongState = CurrentCodeSection.Items[ctx.CodePointer];
                    ctx.State = TestStateEnum.Failed;
                }

                if (ctx.Finished)
                    break;

                if (Delay != 0)
                    Thread.Sleep(Delay);

                if (!ctx.ForceCodePointer)
                    ctx.CodePointer++;
            }

            if (ctx.WrongState == null)
                ctx.State = TestStateEnum.Success;


            if (CurrentCodeSection != Emitter && Finalizer != null)
                foreach (var item in Finalizer.Items)
                    item.Process(ctx);

            if (CurrentCodeSection == Emitter)
                ctx.State = TestStateEnum.Emitter;

            return ctx;
        }

        public void Reset()
        {
           // if (UseEmitter)
               // State = TestStateEnum.Emitter;
        }


        CodeSection IAutoTest.CurrentCodeSection => CurrentCodeSection;

        internal void ParseXml(XElement titem)
        {
            Name = titem.Attribute("name").Value;
            if (titem.Attribute("id") != null)
                Id = int.Parse(titem.Attribute("id").Value);
        }

        public IAutoTest Clone()
        {
            var clone = new SpawnableAutoTest();
            clone.Name = Name;
            clone.Sections.Clear();
            foreach (var item in Sections)
            {
                clone.Sections.Add(item.Clone());
            }

            return clone;
        }

        public XElement ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<spawnableTest id=\"{Id}\" name=\"{Name}\" useEmitter=\"{UseEmitter}\">");

            sb.AppendLine("<vars>");
            foreach (var item in Data)
            {
                sb.AppendLine($"<item key=\"{item.Key}\" value=\"{item.Value}\"/>");
            }
            sb.AppendLine("</vars>");

            foreach (var item in Sections)
                sb.Append(item.ToXml());

            sb.AppendLine("</spawnableTest>");

            return XElement.Parse(sb.ToString());
        }
    }

}
