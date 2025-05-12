using System;
using System.IO;
using System.Text;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace Diplom
{
    public class Compiler
    {
        public string CompileAndExecuteCode(string code)
        {
            try
            {
                // Обёртываем исходный код в метод Main
                string fullCode = $@"
using System;
public class Program
{{
    public static void Main()
    {{
        {code}
    }}
}}";

                var syntaxTree = CSharpSyntaxTree.ParseText(fullCode);

                var references = new[] {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Console).Assembly.Location)
                };

                var compilation = CSharpCompilation.Create(
                    "DynamicCode.dll",
                    new[] { syntaxTree },
                    references,
                    new CSharpCompilationOptions(OutputKind.ConsoleApplication)
                );

                using (var ms = new MemoryStream())
                {
                    EmitResult result = compilation.Emit(ms);

                    if (!result.Success)
                    {
                        StringBuilder errors = new StringBuilder("Ошибки компиляции:\n");
                        foreach (var diagnostic in result.Diagnostics)
                        {
                            errors.AppendLine(diagnostic.ToString());
                        }
                        return errors.ToString();
                    }

                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.ToArray());

                    using (var sw = new StringWriter())
                    {
                        Console.SetOut(sw);
                        var entryPoint = assembly.EntryPoint;
                        entryPoint?.Invoke(null, null);
                        return sw.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Ошибка выполнения: {ex.Message}";
            }
        }
    }
}
