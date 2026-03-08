using System;
using System.Collections.Generic;
using System.IO;
using Lexor;


int passed = 0;
int failed = 0;

void Test(string name, string source, string expected)
{
    var output = new StringWriter();
    Console.SetOut(output);

    Lexora.HadError = false;
    Lexora.HadRuntimeError = false;

    var lexer = new Lexer(source);
    List<Token> tokens = lexer.ScanTokens();

    if (!Lexora.HadError)
    {
        var parser = new Parser(tokens);
        List<Stmt> statements = parser.Parse();

        if (!Lexora.HadError && statements != null)
        {
            var interpreter = new Interpreter();
            interpreter.Interpret(statements);
        }
    }

    Console.SetOut(new StreamWriter(new FileStream("CONOUT$", FileMode.Open, FileAccess.Write)) { AutoFlush = true });

    string actual = output.ToString().Trim();
    bool ok = actual == expected.Trim();

    if (ok)
    {
        passed++;
        Console.WriteLine($"  [PASS] {name}");
    }
    else
    {
        failed++;
        Console.WriteLine($"  [FAIL] {name}");
        Console.WriteLine($"         expected : {expected}");
        Console.WriteLine($"         actual   : {actual}");
    }
}


Console.WriteLine("Running tests...\n");

Test("PRINT: 1 + 1",
    source: """
    SCRIPT AREA
    START SCRIPT
    PRINT: 1 + 1
    END SCRIPT
    """,
    expected: "2"
);

Test("DECLARE INT x = 10 then PRINT x",
    source: """
    SCRIPT AREA
    START SCRIPT
    DECLARE INT x = 10
    PRINT: x
    END SCRIPT
    """,
    expected: "10"
);

Test("PRINT concatenation with sum",
    source: """
    SCRIPT AREA
    START SCRIPT
    DECLARE INT x = 10, y = 20
    DECLARE INT sum
    sum = x + y
    PRINT: "The sum of x and y is: " & sum
    END SCRIPT
    """,
    expected: "The sum of x and y is: 30"
);

Test("PRINT string literal",
    source: """
    SCRIPT AREA
    START SCRIPT
    PRINT: "Hello, World!"
    END SCRIPT
    """,
    expected: "Hello, World!"
);

Test("PRINT: 3 * 4",
    source: """
    SCRIPT AREA
    START SCRIPT
    PRINT: 3 * 4
    END SCRIPT
    """,
    expected: "12"
);


Console.WriteLine($"\n{passed + failed} tests — {passed} passed, {failed} failed.");
