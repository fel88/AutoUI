using System;

namespace AutoUI.Common
{
    public class TestItemEditorAttribute : Attribute
    {
        public Type Target { get; set; }
    }
}
