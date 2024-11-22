using System.Windows.Forms;

namespace AutoUI
{
    public static class UIHelpers
    {
        public static bool ShowQuestion(string text, string title = null)
        {
            return MessageBox.Show(text, title ?? string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}

