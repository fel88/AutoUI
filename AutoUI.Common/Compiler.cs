using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace AutoUI.Common
{
    public class Compiler
    {
        public static RoslynCompilerResults compile(string program)
        {
            List<MetadataReference> References = new List<MetadataReference>();

            List<string> assembliesToBind = new List<string>();
            assembliesToBind.Add("Microsoft.CSharp.dll");
            assembliesToBind.Add("System.Core.dll");
            assembliesToBind.Add("System.Runtime.dll");
            assembliesToBind.Add("System.Collections.dll");
            assembliesToBind.Add("System.Windows.Forms.dll");

            assembliesToBind.Add(Assembly.GetAssembly(typeof(System.Dynamic.DynamicObject)).FullName);
            assembliesToBind.Add(Assembly.GetAssembly(typeof(System.Attribute)).FullName);
            assembliesToBind.Add(Assembly.GetAssembly(typeof(AutoTestRunContext)).FullName);
            
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
            var res = new RoslynCompilerResults(compilation, debug);
            //CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            //ICodeCompiler icc = codeProvider.CreateCompiler();
            //string Output = "Out.dll";


            //System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            ////Make sure we generate an EXE, not a DLL
            //parameters.GenerateExecutable = false;
            //parameters.ReferencedAssemblies.Add("AutoUI.exe");
            //parameters.ReferencedAssemblies.Add("System.Diagnostics.Process.dll");
            //parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            //parameters.ReferencedAssemblies.Add("System.dll");

            ////parameters.OutputAssembly = Output;
            //parameters.GenerateInMemory = true;
            //CompilerResults results = icc.CompileAssemblyFromSource(parameters, program);
            //if (results.Errors.Count > 0)
            //{

            //    /*   textBox2.ForeColor = Color.Red;
            //       foreach (CompilerError CompErr in results.Errors)
            //       {
            //           textBox2.Text = textBox2.Text +
            //                       "Line number " + CompErr.Line +
            //                       ", Error Number: " + CompErr.ErrorNumber +
            //                       ", '" + CompErr.ErrorText + ";" +
            //                       Environment.NewLine + Environment.NewLine;
            //       }*/
            //}
            //else
            //{
            //    /* //Successful Compile
            //     textBox2.ForeColor = Color.Blue;
            //     textBox2.Text = "Success!";
            //     //If we clicked run then launch our EXE
            //     if (ButtonObject.Text == "Run") Process.Start(Output);*/
            //}

            return res;
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
