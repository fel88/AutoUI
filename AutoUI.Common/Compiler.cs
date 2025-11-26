using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;

namespace AutoUI.Common
{
    public class Compiler
    {
        public static RoslynCompilerResults Compile(string program)
        {
            List<MetadataReference> References = new List<MetadataReference>();

            List<string> assembliesToBind = new List<string>();
            assembliesToBind.Add("Microsoft.CSharp.dll");
            assembliesToBind.Add("System.Core.dll");
            assembliesToBind.Add("System.Linq.dll");
            assembliesToBind.Add("System.Runtime.dll");
            assembliesToBind.Add("System.Collections.dll");
            assembliesToBind.Add("System.Windows.Forms.dll");

            assembliesToBind.Add(Assembly.GetAssembly(typeof(System.Dynamic.DynamicObject)).FullName);
            assembliesToBind.Add(Assembly.GetAssembly(typeof(System.Attribute)).FullName);
            assembliesToBind.Add(Assembly.GetAssembly(typeof(AutoTestRunContext)).FullName);
            assembliesToBind.Add(Assembly.GetAssembly(typeof(Task)).FullName);
            assembliesToBind.Add(Assembly.GetAssembly(typeof(IRun)).FullName);

            foreach (var item in assembliesToBind)
            {
                if (File.Exists(item))
                {
                    References.Add(MetadataReference.CreateFromFile(item));
                }
                else
                {
                    Assembly assm = null;
                    try
                    {
                        assm = Assembly.Load(item);
                    }
                    catch (Exception ex)
                    {

                    }
                    if (assm == null)
                    {
                        assm = TryGetAssemblyFromGAC(item);
                    }
                    References.Add(MetadataReference.CreateFromFile(assm.Location));

                }
            }

            var syntaxTree = CSharpSyntaxTree.ParseText(program);
            Random rand = new Random();
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            CSharpCompilation compilation = CSharpCompilation.Create($"assembly_{rand.Next()}_{DateTime.Now.Microsecond}",
                new[] { syntaxTree }, references: References,
                options);
            bool debug = true;
            return new RoslynCompilerResults(compilation, debug);
        }

        private static Assembly TryGetAssemblyFromGAC(string path)
        {
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (item.IsDynamic)
                    continue;

                var d = new DirectoryInfo(Path.GetDirectoryName(item.Location));
                foreach (var ff in d.GetFiles())
                {
                    if (ff.Name.ToLower() == path.ToLower())
                    {
                        return Assembly.LoadFrom(ff.FullName);
                    }
                }
            }
            return null;
        }
    }
}
