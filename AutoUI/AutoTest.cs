using System.Collections.Generic;
using System.Threading;

namespace AutoUI
{
    public class AutoTest
        {
            public int Delay = 0;
            public List<AutoTestItem> Items = new List<AutoTestItem>();
            public void Run()
            {
                foreach (var item in Items)
                {
                    item.Init();
                }
                AutoTestRunContext ctx = new AutoTestRunContext() { Test = this };
                while (ctx.CodePointer < Items.Count && !ctx.Finished)
                {
                    ctx.ForceCodePointer = false;
                    Items[ctx.CodePointer].Process(ctx);
                    if (Delay != 0) { Thread.Sleep(Delay); }
                    if (!ctx.ForceCodePointer)
                        ctx.CodePointer++;
                }
            }
        }


    }
