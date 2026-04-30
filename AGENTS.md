# AGENTS.md

## Build & Run

```bash
dotnet build Lexor/Lexor.csproj          # build
dotnet run --project Lexor/Lexor.csproj  # REPL (no args)
dotnet run --project Lexor/Lexor.csproj -- path/to/script.lxr  # run file
dotnet run --project Lexor/Lexor.csproj -- --test              # run inline tests
```

Targets **.NET 10.0** (not 8.0 — CLAUDE.md has this wrong).

## Architecture

Tree-walk interpreter for the **LEXOR** language, following *Crafting Interpreters*.

```
Source → Lexer.cs → Parser.cs → AST (Expr.cs / Stmt.cs) → Interpreter.cs → output
```

Driver: `Lexora.cs` (`RunFile`, `RunPrompt`). Entry point: `Program.cs`.

## Known Issues

- `Environment` is a single flat scope — no nested scope support yet.
- `Lexora.cs` was previously out of sync with the pipeline (only lexing), but is now correctly wired.

## Implementation Status

| Feature | Lexer | Parser | Interpreter |
|---|---|---|---|
| DECLARE + assignment | Done | Done | Done |
| PRINT | Done | Done | Done |
| Assignment (`=`) | Done | Done | Done |
| Arithmetic / boolean expressions | Done | Done | Done |
| `&` string concat | Done | Done | Done |
| SCAN | Done | Done | **Stub** (`NotImplementedException`) |
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

```
SCRIPT AREA
START SCRIPT
DECLARE INT x = 10
PRINT: "Hello " & x
END SCRIPT
```

Full grammar: `Lexor/Grammar.md`. Increment plan: `Lexor/LEXORIncrementChecks.md`.

## Conventions

- All numeric values are widened to `double` at runtime
- Type keywords (`INT`, `CHAR`, `BOOL`, `FLOAT`) are enforced at runtime (strong typing)
- Bool literals are `TRUE` / `FALSE` (uppercase in output via `Stringify`)
- Error exit codes: 65 (lex/parse), 70 (runtime) — matching Crafting Interpreters
