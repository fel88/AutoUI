using System;
using System.Collections.Generic;

namespace AutoUI.Common
{
    public class EmittedSubTest
    {
        public IAutoTest SourceTest;
        public Dictionary<string, object> Data = new Dictionary<string, object>();
        
        public DateTime FinishTime;
        public DateTime StartTime;
        public TestRunContext lastContext;
        public AutoTestItem WrongState => lastContext.WrongState;
        public TimeSpan Duration => FinishTime - StartTime;

        public TestRunContext Run()
        {
            StartTime = DateTime.Now;

            TestRunContext ctx = new TestRunContext(SourceTest);
            ctx.SubTest = this;
            lastContext = ctx;
            foreach (var item in Data)
            {
                ctx.Vars.Add(item.Key, item.Value);
            }

            SourceTest.Run(ctx);
            //State = SourceTest.State;
            FinishTime = DateTime.Now;
            return ctx;
        }
    }
}
