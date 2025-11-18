using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace AutoUI.Common
{

    public class AutoTest : AbstractAutoTest, IAutoTest
    {
        public AutoTest()
        {

        }
        public AutoTest(TestSet parent, XElement titem) : this()
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

            Name = titem.Attribute("name").Value;            

            if (titem.Attribute("failedAction") != null)
                FailedAction = Enum.Parse<TestFailedBehaviour>(titem.Attribute("failedAction").Value);

            //get all types
            foreach (var section in titem.Elements("section"))
            {
                Code = new CodeSection(this, section);
            }
        }


        public AutoTest(TestSet parent) : this()
        {
            Parent = parent;
            Id = Helpers.GetNewId();
        }

        public TestSet Parent { get; }
        public int Id { get; private set; }
        public string Name { get; set; }
        public int Delay = 0;


        public CodeSection Code = new CodeSection();


        public AutoTestRunContext lastContext;


        public AutoTestRunContext Run(AutoTestRunContext ctx = null)
        {
            if (ctx == null)
                ctx = new AutoTestRunContext() { Test = this };

            if (!ctx.IsSubTest)
            {
                //ctx.Test = this;
                lastContext = ctx;
            }

            foreach (var item in Data)
                ctx.Vars.Add(item.Key, item.Value);

            while (ctx.CodePointer < Code.Items.Count && !ctx.Finished)
            {
                TestItemProcessResultEnum? result = null;
                ctx.ForceCodePointer = false;
                try
                {
                    result = Code.Items[ctx.CodePointer].Process(ctx);
                }
                catch (Exception ex)
                {
                    result = TestItemProcessResultEnum.Exception;
                }
                if (result != TestItemProcessResultEnum.Success)
                {
                    if (FailedAction == TestFailedBehaviour.Terminate)
                        ctx.Finished = true;

                    ctx.WrongState = Code.Items[ctx.CodePointer];
                    State = TestStateEnum.Failed;
                }

                if (ctx.Finished)
                    break;

                if (Delay != 0)
                    Thread.Sleep(Delay);

                if (!ctx.ForceCodePointer)
                    ctx.CodePointer++;
            }

            if (ctx.WrongState == null)
                State = TestStateEnum.Success;


            return ctx;
        }

        public CodeSection CurrentCodeSection { get => Code; }

        public IAutoTest Clone()
        {
            var clone = new AutoTest();
            clone.Name = Name;
            clone.FailedAction = FailedAction;
            clone.Code = Code.Clone();

            return clone;
        }

        public XElement ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<test id=\"{Id}\" name=\"{Name}\" failedAction=\"{FailedAction}\">");

            sb.AppendLine("<vars>");
            foreach (var item in Data)
            {
                sb.AppendLine($"<item key=\"{item.Key}\" value=\"{item.Value}\"/>");
            }
            sb.AppendLine("</vars>");


            sb.Append(Code.ToXml());

            sb.AppendLine("</test>");

            return XElement.Parse(sb.ToString());
        }

        public void Reset()
        {

        }
    }
}
