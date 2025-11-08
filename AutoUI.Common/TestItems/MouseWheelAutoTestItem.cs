using AutoUI.Common;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "wheel")]
    public class MouseWheelAutoTestItem : AutoTestItem
    {
        public override AutoTestItem Clone()
        {
            var ret = new MouseWheelAutoTestItem();
            ret.Delta = Delta;
            return ret;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, int cButtons, uint dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;
        const uint MOUSEEVENTF_WHEEL = 0x0800;
        const uint MOUSEEVENTF_HWHEEL = 0x01000;

        public int Delta { get; set; } = 120;

        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            mouse_event(MOUSEEVENTF_WHEEL, 0, 0, Delta, 0);
            return TestItemProcessResultEnum.Success;
        }

        public override void ParseXml(AutoTest parent, XElement item)
        {
            if (item.Attribute("delta") != null)
                Delta = int.Parse(item.Attribute("delta").Value);

            base.ParseXml(parent, item);
        }
        public override string ToXml()
        {
            return $"<wheel delta=\"{Delta}\"/>";
        }
    }
}
