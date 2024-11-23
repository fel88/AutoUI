using System;

namespace AutoUI
{
    public class TestItemEditorAttribute : Attribute
    {
        public Type Editor { get; set; }
    }
}
