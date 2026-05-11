# AGENTS.md

## Build & Run

```bash
dotnet build Lexor/Lexor.csproj          # build
dotnet run --project Lexor/Lexor.csproj  # REPL (double-enter to end input)
dotnet run --project Lexor/Lexor.csproj -- path/to/script.lxr  # run file
dotnet run --project Lexor/Lexor.csproj -- --test              # run inline tests
```

Targets **.NET 10.0** (CLAUDE.md incorrectly says 8.0).

## Architecture

Tree-walk interpreter for **LEXOR**, following *Crafting Interpreters*.

```
Source → Lexer.cs → Parser.cs → AST (Expr.cs / Stmt.cs) → Interpreter.cs → output
```

Driver: `Lexora.cs` (`RunFile`, `RunPrompt`). Entry point: `Program.cs`.

## Implementation Status

| Feature | Lexer | Parser | Interpreter |
|---|---|---|---|
| DECLARE + assignment | Done | Done | Done |
| PRINT | Done | Done | Done |
| Assignment (`=`) | Done | Done | Done |
| Arithmetic / boolean expressions | Done | Done | Done |
| `&` string concat | Done | Done | Done |
| SCAN | Done | Done | **Stub** |
| IF / ELSE IF / ELSE | Done | **Not written** | Stub |
| FOR | Done | **Not written** | Stub |
| REPEAT WHEN | Done | **Not written** | Stub |
| Block statements | — | **Not written** | Stub |

## Adding a New Statement

1. Add token types to `TokenType.cs` if needed
2. Add AST node in `Stmt.cs` with `Accept` override
3. Add `VisitXxxStmt` to `Stmt.IVisitor<R>` and stub in `Interpreter.cs`
4. Parse it in `Parser.cs` inside `Statement()`
5. Implement `VisitXxxStmt` in `Interpreter.cs`
6. Add test in `Program.cs` `RunTests()`

## Language Syntax

Programs must be wrapped in `SCRIPT AREA / START SCRIPT … END SCRIPT`.
Keywords are ALL-CAPS. Comments use `%%`. `$` is a newline literal in PRINT.
`&` concatenates values. `[<char>]` is an escape code in PRINT prints.

```
SCRIPT AREA
START SCRIPT
DECLARE INT x = 10
PRINT: "Hello " & x
END SCRIPT
```

Full grammar: `Lexor/Grammar.md`. Language spec: `Lexor/specifics.md`.
Increment plan: `Lexor/LEXORIncrementChecks.md`.

## Conventions

- All numeric values are widened to `double` at runtime
- Type keywords (`INT`, `CHAR`, `BOOL`, `FLOAT`) are enforced at runtime (strong typing)
- Bool output is `TRUE` / `FALSE` (uppercase via `Stringify`)
- Error exit codes: 64 (usage), 65 (lex/parse), 70 (runtime)
- `Environment` is a single flat `Dictionary` — no nested scope yet

## Notes

- No CI, linter, or formatter configuration exists
- `tool/GenerateAst.cs` is excluded from compilation via `<Compile Remove>`
- `.lxr` test files in `tests/` exist but are NOT part of the automated test suite — only inline tests in `Program.cs` `RunTests()` run with `--test`
