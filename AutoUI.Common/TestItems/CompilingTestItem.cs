using AutoUI.Common;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.Common.TestItems
{
    [XmlParse(XmlKey = "custom")]
   
    public class CompilingTestItem : AutoTestItem
    {
        public string ProgramText;
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            var results = compile();

            
            foreach (var item in results.Errors.OfType<CompilerError>())
            {
                return TestItemProcessResultEnum.Failed;
            }
            try
            {
                Assembly asm = results.CompiledAssembly;

                Type[] allTypes = results.CompiledAssembly.GetTypes();

                foreach (Type t in allTypes.Take(1))
                {
                    var inst = Activator.CreateInstance(t);
                    //dynamic v = inst;
                    var mf = t.GetMethods().FirstOrDefault(z => z.Name.Contains("sum"));
                    var mf2 = t.GetMethods().FirstOrDefault(z => z.Name.Contains("run"));
                    if (mf != null)
                    {
                        var res = mf.Invoke(inst, new object[] { 3, 5 });
                        //MessageBox.Show(res + "");
                    }
                    if (mf2 != null)
                    {
                        
                        mf2.Invoke(inst, new object[] { ctx });
                        //MessageBox.Show(ctx.Vars["temp"] + "");
                    }
                    else
                    {
                        return TestItemProcessResultEnum.Failed;//not found entry point
                    }
                    //MessageBox.Show(v.sum(3, 5));
                    //TryLoadCompiledType(res, t.ToString());
                    //Debug.WriteLine(t.ToString());
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return TestItemProcessResultEnum.Failed;
            }

            return TestItemProcessResultEnum.Success;
        }
        public override void ParseXml(AutoTest parent, XElement item)
        {
            ProgramText = item.Value;            
            
            base.ParseXml(parent, item);
        }
        public CompilerResults compile()
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            string Output = "Out.dll";
            

            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            //Make sure we generate an EXE, not a DLL
            parameters.GenerateExecutable = false;
            parameters.ReferencedAssemblies.Add("AutoUI.exe");
            parameters.ReferencedAssemblies.Add("System.Diagnostics.Process.dll");
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add("System.dll");
            
            //parameters.OutputAssembly = Output;
            parameters.GenerateInMemory = true;
            CompilerResults results = icc.CompileAssemblyFromSource(parameters, ProgramText);
            if (results.Errors.Count > 0)
            {
                
             /*   textBox2.ForeColor = Color.Red;
                foreach (CompilerError CompErr in results.Errors)
                {
                    textBox2.Text = textBox2.Text +
                                "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine;
                }*/
            }
            else
            {
                /* //Successful Compile
                 textBox2.ForeColor = Color.Blue;
                 textBox2.Text = "Success!";
                 //If we clicked run then launch our EXE
                 if (ButtonObject.Text == "Run") Process.Start(Output);*/
            }

            return results;
        }
        public override string ToXml()
        {            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<custom>");
            sb.AppendLine($"<![CDATA[{ProgramText}]]>");
            sb.AppendLine("</custom>");
            return sb.ToString();
        }
    }
}
