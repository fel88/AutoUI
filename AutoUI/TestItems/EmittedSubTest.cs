using System;
using System.Collections.Generic;

namespace AutoUI.TestItems
{
    public class EmittedSubTest
    {
        public AutoTest SourceTest;
        public Dictionary<string, object> Data = new Dictionary<string, object>();
        public TestStateEnum State;
        public DateTime FinishTime;
        public DateTime StartTime;
        public AutoTestRunContext lastContext;
        public AutoTestItem WrongState => lastContext.WrongState;
        public TimeSpan Duration => FinishTime - StartTime;

        internal AutoTestRunContext Run()
        {
            StartTime = DateTime.Now;

            AutoTestRunContext ctx = new AutoTestRunContext() { Test = SourceTest };
            ctx.SubTest = this;
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
}
