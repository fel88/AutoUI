using System;
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

        internal static bool Question(string text,string caption)
        {
            return MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}