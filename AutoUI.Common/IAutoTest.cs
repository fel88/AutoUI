using System.Xml.Linq;

namespace AutoUI.Common
{
    public interface IAutoTest
    {        
        TestRunContext Run(TestRunContext ctx = null);
        CodeSection CurrentCodeSection { get; }
        IAutoTest Clone();
        Dictionary<string, object> Data { get; set; }

        string Name { get; set; }
        
        TestSet Parent { get; }
        XElement ToXml();
    }
}
