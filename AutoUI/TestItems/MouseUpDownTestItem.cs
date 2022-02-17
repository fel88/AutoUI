using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "mouseUpDown")]
    public class MouseUpDownTestItem : AutoTestItem
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public void DoMouseClick()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
        public void DoMouseDown()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
        }
        public void DoMouseUp()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
        public bool Down { get; set; }
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            if (Down)
            {
                DoMouseDown();
            }
            else
            {
                DoMouseUp();
            }

            return TestItemProcessResultEnum.Success;
        }

        public override void ParseXml(XElement item)
        {
            Down = bool.Parse(item.Attribute("down").Value);
            base.ParseXml(item);
        }
        internal override string ToXml()
        {
            return $"<mouseUpDown down=\"{Down}\"/>";
        }
    }
}