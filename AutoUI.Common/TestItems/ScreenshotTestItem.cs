using AutoUI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "screenshot")]
    public class ScreenshotTestItem : AutoTestItem
    {
        public bool ReportWindow(IntPtr hwnd, int lParam)
        {
            uint processId = 0;
            uint threadId = GetWindowThreadProcessId(hwnd, out processId);

            RECT rt = new RECT();
            bool locationLookupSucceeded = GetWindowRect(hwnd, out rt);
            return false;
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            Image scr = null;            
            if (ProcessOnly)
            {
                //EnumWindowsCallback callBackFn = new EnumWindowsCallback(ReportWindow);
                //EnumWindows(callBackFn, 0);
                var p = System.Diagnostics.Process.GetProcessById((int)ctx.Vars[ProcessRegisterKey]);
                //Rectangle biggest = new Rectangle();
                //int cntr = 0;
                //foreach (var handle in EnumerateProcessWindowHandles(p.Id))
                //{
                //    var rect = new User32.Rect();

                //    User32.GetWindowRect(handle, ref rect);
                //    if (biggest.Width > 0 && biggest.Height > 0)
                //    {
                //        var test1 = CaptureApplication(biggest);
                //        if (ctx.SubTest != null)
                //        {
                //            ctx.SubTest.Data.Add(RegisterKey + "" + cntr++, test1);
                //        }
                //    }

                //    if ((rect.right - rect.left) > biggest.Width)
                //    {
                //        biggest = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
                //    }
                //}
                //{


                //scr = CaptureApplication(rect);
                //scr = CaptureApplication(biggest);
                scr = CaptureApplication(p);
            }
            else
                scr = SearchByPatternImage.GetScreenshot();

            if (ctx.SubTest != null)
            {
                ctx.SubTest.Data.Add(RegisterKey, scr);
            }
            return TestItemProcessResultEnum.Success;
        }

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);
        delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);
        // Callback Declaration
        public delegate bool EnumWindowsCallback(IntPtr hwnd, int lParam);
        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWindowsCallback callPtr, int lParam);

        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn,
            IntPtr lParam);

        static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId)
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in System.Diagnostics.Process.GetProcessById(processId).Threads)
                EnumThreadWindows(thread.Id,
                    (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles;
        }
        public Bitmap CaptureApplication(Process proc)
        {
            var rect = new User32.Rect();
            User32.GetWindowRect(proc.MainWindowHandle, ref rect);


            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            var bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
            }

            return bmp;
        }

        public Bitmap CaptureApplication(Rectangle rect)
        {
            int width = rect.Width;
            int height = rect.Height;

            var bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
            }

            return bmp;
        }

        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Rect
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
        }

        public bool ProcessOnly { get; set; }

        public string ProcessRegisterKey { get; set; }

        public string RegisterKey { get; set; }        

        public override void ParseXml(IAutoTest set, XElement item)
        {
            RegisterKey = item.Attribute("key").Value;
            ProcessOnly = bool.Parse(item.Attribute("processOnly").Value);
            ProcessRegisterKey = item.Attribute("pidKey").Value;
            base.ParseXml(set, item);
        }

        public override string ToXml()
        {
            return $"<screenshot processOnly=\"{ProcessOnly}\" pidKey=\"{ProcessRegisterKey}\" key=\"{RegisterKey}\" />";
        }
    }
}