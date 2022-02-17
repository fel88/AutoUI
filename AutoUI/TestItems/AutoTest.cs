using System.Collections.Generic;
using System.Threading;

namespace AutoUI.TestItems
{
    public class AutoTest
    {

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
                }
                if (ctx.Finished) break;

                if (Delay != 0) { Thread.Sleep(Delay); }
                if (!ctx.ForceCodePointer)
                    ctx.CodePointer++;
            }
            return ctx;

        }
        public TestStateEnum State;
    }

    public enum TestStateEnum
    {
        NotStarted, Failed, Success
    }

}
