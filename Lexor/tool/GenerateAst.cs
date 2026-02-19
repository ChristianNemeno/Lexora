using System;

namespace Lexor.tool;

public class GenerateAst
{
    static void Main(string[] args)
    {
        if (args.Length == 1)
        {
            Console.Error.WriteLine("Usage: generate_ast <output directory>");
            Environment.Exit(64);
        }
        var outputDir = args[0];

        DefineAst(outputDir, "Expr", new List<string>
        {
            "Binary : Expr left, Token operator, Expr right",
            "Grouping : Expr expression",
            "Literal : Object value",
            "Unary : Token operator, Expr right"
        });
    }

    private static void DefineAst(string outputDir, string baseName, List<string> types)
    {
        var path = Path.Combine(outputDir, baseName + ".cs");

        using var writer = new StreamWriter(path, false, System.Text.Encoding.UTF8);
        writer.WriteLine("namespace Lexor;");
        writer.WriteLine();
        writer.WriteLine("using System.Collections.Generic;");
        writer.WriteLine();
        writer.WriteLine($"abstract class {baseName}");
        writer.WriteLine("{");
        foreach (var type in types)
        {
            string className = type.Split(":")[0].Trim();
            string fields = type.Split(":")[1].Trim();
            DefineType(writer, baseName, className, fields);
        }
        writer.WriteLine("}");
    }

    private static void DefineType(
    StreamWriter writer, string baseName,
    string className, string fieldList)
    {
        writer.WriteLine($"    static class {className} : {baseName}");
        writer.WriteLine("    {");

        // constructor
        writer.WriteLine($"        {className}({fieldList})");
        writer.WriteLine("        {");

        // store parameters in fields
        var fields = fieldList.Split(", ");
        foreach (var field in fields)
        {
            // e.g. "int value" -> parts[1] == "value"
            var parts = field.Split(' ');
            var name = parts[1];
            writer.WriteLine($"            this.{name} = {name};");
        }

        writer.WriteLine("        }");
        writer.WriteLine();

        // fields
        foreach (var field in fields)
        {
            writer.WriteLine($"        readonly {field};");
        }

        writer.WriteLine("        }");
    }
}
