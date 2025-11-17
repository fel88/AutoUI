using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;

namespace AutoUI.Common
{
    public class RoslynCompilerResults
    {
        public List<CompilerEventInfo> Errors = new List<CompilerEventInfo>();
        public Assembly Assembly;
        readonly CSharpCompilation _compilationResults;
        public RoslynCompilerResults(CSharpCompilation compilation, bool debug)
        {
            _compilationResults = compilation;
            using var ms = new MemoryStream();
            var emitOpetions = debug ? new EmitOptions(debugInformationFormat: DebugInformationFormat.Embedded) : new EmitOptions();
            EmitResult result = _compilationResults.Emit(ms, options: emitOpetions);
            if (result.Success)
            {
                ms.Seek(0, SeekOrigin.Begin);
                Assembly = Assembly.Load(ms.ToArray());
            }
            else
            {
                IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                diagnostic.IsWarningAsError ||
                diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (var item in failures)
                {
                    var l = _compilationResults.SyntaxTrees[0].GetLineSpan(item.Location.SourceSpan);
                    var line = l.StartLinePosition.Line;
                    Errors.Add(new CompilerEventInfo() { Line = line + 1, Text = item.GetMessage() });
                }
            }
        }
    }
}
