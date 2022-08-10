using System;
using System.Threading;
using System.Windows.Forms;

namespace AutoUI.TestItems
{
    public static class Helpers
    {
        public static int NewId = 0;
        public static int GetNewId()
        {
            return NewId++;
        }

        internal static bool Question(string text, string caption)
        {
            return MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        internal static void ExecuteSTA(Action p)
        {
            Thread th = new Thread(() =>
            {
                p();
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            th.Join();
        }
    }
}