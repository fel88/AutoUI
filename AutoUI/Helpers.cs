using System;
using System.Threading;
using System.Windows.Forms;

namespace AutoUI
{
    public static class Helpers
    {
       
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

        internal static void Error(string v)
        {
            MessageBox.Show(v, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}