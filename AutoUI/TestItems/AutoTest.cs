using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    public class AutoTest
    {
        public AutoTest()
        {

        }


        
        public AutoTest(TestSet parent)
        {
            Parent = parent;
            Id = Helpers.GetNewId();
        }
        public TestSet Parent;
        public int Id { get; private set; }
        public string Name { get; set; }
        public int Delay = 0;
        public List<AutoTestItem> Items = new List<AutoTestItem>();
        public AutoTestRunContext Run()
        {
            foreach (var item in Items)
            {
                item.Init();
            }



            AutoTestRunContext ctx = new AutoTestRunContext() { Test = this };
            while (ctx.CodePointer < Items.Count && !ctx.Finished)
            {
                ctx.ForceCodePointer = false;
                var result = Items[ctx.CodePointer].Process(ctx);
                if (result == TestItemProcessResultEnum.Failed)
                {
                    ctx.Finished = true;
                    ctx.WrongState = Items[ctx.CodePointer];
                    State = TestStateEnum.Failed;
                }
                if (ctx.Finished) break;

                if (Delay != 0) { Thread.Sleep(Delay); }
                if (!ctx.ForceCodePointer)
                    ctx.CodePointer++;
            }
            if (ctx.WrongState == null)
            {
                State = TestStateEnum.Success;
            }
            return ctx;

        }
        public TestStateEnum State;

        internal void ParseXml(XElement titem)
        {
            Name = titem.Attribute("name").Value;
            if (titem.Attribute("id") != null)
                Id = int.Parse(titem.Attribute("id").Value);
        }
    }

    public enum TestStateEnum
    {
        NotStarted, Failed, Success
    }

}
