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

        public Dictionary<string, object> Data = new Dictionary<string, object>();
        public TestFailedbehaviour FailedAction { get; set; }
        
        public AutoTest(TestSet parent)
        {
            Parent = parent;
            Id = Helpers.GetNewId();
        }
        public TestSet Parent;
        public int Id { get; private set; }
        public string Name { get; set; }
        public int Delay = 0;

        public bool UseEmitter { get; set; } = false;
        public CodeSection Finalizer = new CodeSection();
        public CodeSection Main = new CodeSection();
        public CodeSection Emitter = new CodeSection();

        public AutoTestRunContext lastContext;
        public AutoTestRunContext Run(AutoTestRunContext ctx = null)
        {
            CodeSection main = Main;
            if (UseEmitter)
        {
                main = Emitter;
                State = TestStateEnum.Emitter;
            }
            if (ctx != null && ctx.IsSubTest)
            {
                main = Main;
            }
            foreach (var item in main.Items)
            {
                item.Init();
            }

            if (ctx == null)
            {
                ctx = new AutoTestRunContext() { Test = this };
            }
            if (!ctx.IsSubTest)
            {
                //ctx.Test = this;
                lastContext = ctx;
            }
            foreach (var item in Data)
            {
                ctx.Vars.Add(item.Key, item.Value);
            }
            while (ctx.CodePointer < main.Items.Count && !ctx.Finished)
            {
                ctx.ForceCodePointer = false;
                var result = main.Items[ctx.CodePointer].Process(ctx);
                if (result == TestItemProcessResultEnum.Failed)
                {
                    if (FailedAction == TestFailedbehaviour.Terminate)
                    {
                    ctx.Finished = true;
                    }
                    ctx.WrongState = main.Items[ctx.CodePointer];
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

            if (main != Emitter)
                foreach (var item in Finalizer.Items)
                {
                    item.Process(ctx);
                }

            if (main == Emitter)
            {
                State = TestStateEnum.Emitter;
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
        NotStarted, Failed, Success, Emitter
    }

    public class EmittedSubTest
    {
        public AutoTest SourceTest;
        public Dictionary<string, object> Data = new Dictionary<string, object>();
        public TestStateEnum State;
        public DateTime FinishTime;
        public AutoTestRunContext lastContext;
        public AutoTestItem WrongState => lastContext.WrongState;

        internal AutoTestRunContext Run()
        {
            AutoTestRunContext ctx = new AutoTestRunContext() { Test = SourceTest };
            ctx.IsSubTest = true;
            lastContext = ctx;
            foreach (var item in Data)
            {
                ctx.Vars.Add(item.Key, item.Value);
    }

            SourceTest.Run(ctx);
            State = SourceTest.State;
            FinishTime = DateTime.Now;
            return ctx;
        }
    }

    public enum TestFailedbehaviour
    {
        Terminate, Ignore
    }
    public class CodeSection
    {
        public List<AutoTestItem> Items = new List<AutoTestItem>();
    }
}
