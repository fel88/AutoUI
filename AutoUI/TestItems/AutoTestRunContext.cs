using System.Collections.Generic;
using System.Drawing;

namespace AutoUI.TestItems
{
    public class AutoTestRunContext
    {
        public AutoTest Test;
        public int CodePointer;
        public bool ForceCodePointer;
        public AutoTestItem WrongState;
        public bool Finished;

        public Point? LastSearchPosition;
        public Dictionary<string, object> Vars = new Dictionary<string, object>();
    }
}
