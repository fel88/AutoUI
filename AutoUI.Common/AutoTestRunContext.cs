using AutoUI.TestItems;
using System.Collections.Generic;
using System.Drawing;

namespace AutoUI.Common
{
    public class AutoTestRunContext
    {
        public void JumpToLabel(string label)
        {
            var fr = Test.CurrentCodeSection.Items.OfType<LabelAutoTestItem>().First(z => z.Label == label);
            CodePointer = Test.CurrentCodeSection.Items.IndexOf(fr);
            ForceCodePointer = true;
        }

        // Example for retrieving clipboard text safely in a non-STA thread (e.g., console app):
        [STAThread] // Mark the Main method with this attribute for console apps
        public static string GetClipboardTextSafe()
        {
            string clipboardText = string.Empty;
            Exception threadEx = null;

            Thread staThread = new Thread(
                delegate ()
                {
                    try
                    {
                        if (Clipboard.ContainsText(TextDataFormat.Text))
                        {
                            clipboardText = Clipboard.GetText(TextDataFormat.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        threadEx = ex;
                    }
                });

            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();

            if (threadEx != null)
            {
                // Handle or re-throw the exception that occurred in the STA thread
                throw threadEx;
            }

            return clipboardText;
        }

        public IAutoTest Test;
        public int CodePointer;
        public bool ForceCodePointer;
        public AutoTestItem WrongState;
        public bool Finished;
        public bool IsSubTest => SubTest != null;
        public EmittedSubTest SubTest;

        public Point? LastSearchPosition;
        public Dictionary<string, string> StringRegisters = new Dictionary<string, string>();
        public Dictionary<string, object> Registers = new Dictionary<string, object>();
        public Dictionary<object, object> TagRegisters = new Dictionary<object, object>();
        public Dictionary<string, int> IntRegisters = new Dictionary<string, int>();

        public Dictionary<string, object> Vars = new Dictionary<string, object>();
        public List<EmittedSubTest> SubTests = new List<EmittedSubTest>();
        public void EmitSubTest(KeyValuePair<string, object>[] vars)
        {
            var et = new EmittedSubTest() { SourceTest = Test };
            SubTests.Add(et);

            if (vars != null)
                foreach (var item in vars)
                {
                    et.Data.Add(item.Key, item.Value);
                }
        }
    }
}
