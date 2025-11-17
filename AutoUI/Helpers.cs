using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Threading;
using System.Windows.Forms;

namespace AutoUI
{
    public static class Helpers
    {
        public static string NormalizeCodeWithRoslyn(this string csCode)
        {
            var tree = CSharpSyntaxTree.ParseText(csCode);
            var root = tree.GetRoot().NormalizeWhitespace();
            return root.ToFullString();
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

        internal static void Error(string v)
        {
            MessageBox.Show(v, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}