using System.Globalization;

namespace AutoUI.Common
{
    public static class Helpers
    {
        public static int NewId = 0;
        public static int GetNewId()
        {
            return NewId++;
        }

        public static double ToDouble(this string str) => double.Parse(str.Replace(",", "."), CultureInfo.InvariantCulture);
    }
}