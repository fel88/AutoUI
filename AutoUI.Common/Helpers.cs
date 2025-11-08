namespace AutoUI.Common
{
    public static class Helpers
    {
        public static int NewId = 0;
        public static int GetNewId()
        {
            return NewId++;
        }
    }
    }