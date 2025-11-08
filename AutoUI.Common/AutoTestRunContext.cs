using System.Collections.Generic;
using System.Drawing;

namespace AutoUI.Common
{
    public class AutoTestRunContext
    {
        public AutoTest Test;
        public int CodePointer;
        public bool ForceCodePointer;
        public AutoTestItem WrongState;
        public bool Finished;
        public bool IsSubTest => SubTest != null;
        public EmittedSubTest SubTest;

        public Point? LastSearchPosition;
        public Dictionary<string, object> Vars = new Dictionary<string, object>();
        public List<EmittedSubTest> SubTests = new List<EmittedSubTest>();
        public void EmitSubTest(KeyValuePair<string, object>[] vars)
        {
            var et = new EmittedSubTest() { SourceTest = Test };
            SubTests.Add(et);
            foreach (var item in vars)
            {
                et.Data.Add(item.Key, item.Value);
            }
        }
    }
}
