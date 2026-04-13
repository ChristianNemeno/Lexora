# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

```bash
# Build
dotnet build Lexor/Lexor.csproj

# Run tests (defined inline in Program.cs)
dotnet run --project Lexor/Lexor.csproj
```

The project targets .NET 8.0. All source is under `Lexor/`.

## Architecture

This is a tree-walk interpreter for a custom language called **LEXOR**, following the classic pipeline:

```
Source Text → Lexer → [Token] → Parser → [Stmt/Expr AST] → Interpreter → output
```

### Pipeline Components

- **`Lexer.cs`** — Scans source text into tokens. Keywords are all-caps (`DECLARE`, `PRINT`, `IF`, etc.). Comments use `%%`. `$` is a newline literal in print.
- **`Parser.cs`** — Recursive descent parser. Entry point is `Parse()`, which expects the `SCRIPT AREA / START SCRIPT … END SCRIPT` wrapper. Declarations (`DECLARE`) must come before executable statements.
- **`Expr.cs`** — AST expression nodes: `Literal`, `Binary`, `Unary`, `Grouping`, `Variable`, `Assign`. Uses the Visitor pattern (`IVisitor<R>` / `Accept`).
- **`Stmt.cs`** — AST statement nodes: `Declare`, `Expression`, `Print`, `Scan`, `If`, `For`, `Repeat`, `Block`. Same Visitor pattern.
- **`Interpreter.cs`** — Implements `Expr.IVisitor<object?>` and `Stmt.IVisitor<object?>`. Walks the AST and executes it. All numeric values are widened to `double` internally.
- **`Environment.cs`** — Single flat `Dictionary<string, object?>` for variable storage. No scope nesting yet.
- **`Lexora.cs`** — Static driver: `RunFile(path)` and `RunPrompt()` (REPL). Holds `HadError` / `HadRuntimeError` flags. **Note:** `Lexora.Run()` currently only prints tokens — the Parser/Interpreter calls are commented out. The working pipeline is wired together only in `Program.cs` (the test harness).

### What's implemented vs. stub

| Feature | Status |
|---|---|
| Lexing | Done |
| `DECLARE` + assignment | Done |
| `PRINT:` | Done |
| Arithmetic & boolean expressions | Done |
| `&` string concatenation | Done |
| `SCAN:` | Parser done, interpreter stub (`NotImplementedException`) |
| `IF` / `ELSE IF` / `ELSE` | AST nodes defined, parser not yet written, interpreter stub |
| `FOR` loop | AST node defined, parser not yet written, interpreter stub |
| `REPEAT WHEN` loop | AST node defined, parser not yet written, interpreter stub |

### Adding a new statement

1. Add token types to `TokenType.cs` if needed.
2. Add the AST node class inside `Stmt.cs` (or `Expr.cs`), implementing `Accept`.
3. Add `VisitXxxStmt` to the `Stmt.IVisitor<R>` interface and add a stub to `Interpreter.cs`.
4. Parse it in `Parser.cs` inside `Statement()`.
5. Implement `VisitXxxStmt` in `Interpreter.cs`.
6. Add a test case in `Program.cs`.

## Language Syntax Reference

See `Lexor/Grammar.md` for the full CFG. Quick example:

```
SCRIPT AREA
START SCRIPT
DECLARE INT x = 10, y = 20
PRINT: "Sum: " & x + y
END SCRIPT
```

## Reference
This project follows the book "Crafting Interpreters" by Robert Nystrom. Prefer the book's patterns and naming conventions when implementing features.