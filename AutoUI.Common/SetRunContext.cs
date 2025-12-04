namespace AutoUI.Common
{
    public class SetRunContext
    {
        public SetRunContext(TestSet parent)
        {
            Parent = parent;
        }

        public TestSet Parent { get; private set; }


    }
}

