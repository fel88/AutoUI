using System.Drawing;

namespace AutoUI
{
    public class AutoTestRunContext
    {
        public AutoTest Test;
        public int CodePointer;
        public bool ForceCodePointer;
        public bool Finished;

        public Point? LastSearchPosition;
    }
}
