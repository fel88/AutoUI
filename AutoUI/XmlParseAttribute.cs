using System;

namespace AutoUI
{
    public class XmlParseAttribute : Attribute
    {
        public string XmlKey { get; set; }
    }

    public class TestItemEditorAttribute : Attribute
    {
        public Type Editor { get; set; }
    }
}
