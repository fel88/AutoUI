using AutoUI.Common;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "click")]
    public class ClickAutoTestItem : AutoTestItem
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public bool IsRight { get; set; } = false;
        public bool DoubleClick { get; set; } = false;
        public void DoMouseClick()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            if (IsRight)
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
            }
            else { mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0); }
        }
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            DoMouseClick();
            if (DoubleClick)
            {
                Thread.Sleep(50);
                DoMouseClick();
            }
            return TestItemProcessResultEnum.Success;
                
        }

        public override void ParseXml(AutoTest set, XElement item)
        {
            if (item.Attribute("double") != null)
            {
                DoubleClick = bool.Parse(item.Attribute("double").Value);
            }
            if (item.Attribute("isRight") != null)
            {
                IsRight= bool.Parse(item.Attribute("isRight").Value);
            }
            base.ParseXml(set, item);
        }
        public override string ToXml()
        {
            return $"<click double=\"{DoubleClick}\" isRight=\"{IsRight}\"/>";
        }
    }
}
