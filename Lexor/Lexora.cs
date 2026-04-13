using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lexor;

internal static class Lexora
{
    public static bool HadError = false;
    public static bool HadRuntimeError = false;


    public static void RunFile(string path)
    {
        string source = File.ReadAllText(path, Encoding.Default);
        Run(source);

        if (HadError) System.Environment.Exit(65);
        if (HadRuntimeError) System.Environment.Exit(70);
    }

    public static void RunPrompt(string source)
    {
        Run(source);
        HadError = false;
        HadRuntimeError = false;
    }


    private static void Run(string source)
    {
        Lexer lexer = new(source);
        List<Token> tokens = lexer.ScanTokens();
        if (HadError) return;

        Parser parser = new(tokens);
        List<Stmt> statements = parser.Parse();
        if (HadError) return;

        Interpreter interpreter = new();
        interpreter.Interpret(statements);
    }


    /// <summary>Reports an error at a given line number.</summary>
    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    /// <summary>Reports an error at a given token.</summary>
    public static void Error(Token token, string message)
    {
        if (token.Type == TokenType.Eof)
            Report(token.Line, " at end", message);
        else
            Report(token.Line, $" at '{token.Lexeme}'", message);
    }

    /// <summary>Reports a runtime error.</summary>
    public static void RuntimeError(RuntimeError error)
    {
        Console.Error.WriteLine($"{error.Message}\n[line {error.Token.Line}]");
        HadRuntimeError = true;
    }


    private static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
        HadError = true;
    }
}