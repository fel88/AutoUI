using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace AutoUI.Common
{
    public class AutoTest
    {
        public AutoTest()
        {

        }

        public Dictionary<string, object> Data = new Dictionary<string, object>();
        public TestFailedBehaviour FailedAction { get; set; }

        public AutoTest(TestSet parent) : this()
        {
            Parent = parent;
            Id = Helpers.GetNewId();
        }

        public readonly TestSet Parent;
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
                State = TestStateEnum.Emitter;
            }

            if (ctx != null && ctx.IsSubTest)
                CurrentCodeSection = Main;

            foreach (var item in CurrentCodeSection.Items)
                item.Init();


            if (ctx == null)
                ctx = new AutoTestRunContext() { Test = this };

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


            if (CurrentCodeSection != Emitter && Finalizer != null)
                foreach (var item in Finalizer.Items)
                    item.Process(ctx);

            if (CurrentCodeSection == Emitter)
                State = TestStateEnum.Emitter;

            return ctx;
        }

        public TestStateEnum State;

        internal void ParseXml(XElement titem)
        {
            Name = titem.Attribute("name").Value;
            if (titem.Attribute("id") != null)
                Id = int.Parse(titem.Attribute("id").Value);
        }

        public AutoTest Clone()
        {
            var clone = new AutoTest();
            clone.Name = Name;
            clone.Sections.Clear();
            foreach (var item in Sections)
            {
                clone.Sections.Add(item.Clone());
            }

            return clone;
        }
    }
}
