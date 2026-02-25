# LEXOR Language — Context-Free Grammar (CFG)

> Notation: `->` means "is defined as", `|` means "or", `ε` means empty/optional.

---

## Program Structure

```
<Program> -> "SCRIPT AREA" "START SCRIPT" <Declarations> <Executable_Codes> "END SCRIPT"

<Declarations> -> <Declaration> <Declarations> | ε

<Declaration> -> "DECLARE" <Data_Type> <Var_List>

<Data_Type> -> "INT" | "CHAR" | "BOOL" | "FLOAT"

<Var_List> -> <Var_Init> | <Var_Init> "," <Var_List>

<Var_Init> -> <Identifier> | <Identifier> "=" <Expression>
```

---

## Executable Codes & Statements

```
<Executable_Codes> -> <Statement> <Executable_Codes> | ε

<Statement> -> <Assignment>
              | <Print_Statement>
              | <Scan_Statement>
              | <If_Statement>
              | <For_Loop>
              | <Repeat_Loop>

<Assignment> -> <Identifier> "=" <Expression>
               | <Identifier> "=" <Assignment>
```

---

## Input / Output

```
<Print_Statement> -> "PRINT:" <Print_List>

<Print_List> -> <Print_Item> | <Print_Item> "&" <Print_List>

<Print_Item> -> <Expression>
               | "$"
               | "[" <Char_Literal> "]"

<Scan_Statement> -> "SCAN:" <Id_List>

<Id_List> -> <Identifier> | <Identifier> "," <Id_List>
```

---

## Control Flow

```
<If_Statement> -> "IF" "(" <Bool_Expression> ")" 
                       "START IF" <Executable_Codes> "END IF"
                       <Else_If_List>
                       <Else_Block>

<Else_If_List> -> "ELSE IF" "(" <Bool_Expression> ")"
                       "START IF" <Executable_Codes> "END IF"
                       <Else_If_List>
                 | ε

<Else_Block> -> "ELSE" "START IF" <Executable_Codes> "END IF" | ε

<For_Loop> -> "FOR" "(" <Assignment> "," <Bool_Expression> "," <Assignment> ")"
                   "START FOR" <Executable_Codes> "END FOR"

<Repeat_Loop> -> "REPEAT WHEN" "(" <Bool_Expression> ")"
                      "START REPEAT" <Executable_Codes> "END REPEAT"
```

---

## Expressions

```
<Expression> -> <Bool_Expression> | <Arith_Expression> | <String_Literal>

<Bool_Expression> -> <Bool_Term> | <Bool_Term> "OR" <Bool_Expression>

<Bool_Term> -> <Bool_Factor> | <Bool_Factor> "AND" <Bool_Term>

<Bool_Factor> -> "NOT" <Relational_Expression>
                | <Relational_Expression>
                | <Bool_Literal>

<Relational_Expression> -> <Arith_Expression> <Relational_Op> <Arith_Expression>
                           | "(" <Bool_Expression> ")"
                           | <Arith_Expression>

<Relational_Op> -> ">" | "<" | ">=" | "<=" | "==" | "<>"

<Arith_Expression> -> <Term>
                      | <Term> "+" <Arith_Expression>
                      | <Term> "-" <Arith_Expression>

<Term> -> <Factor>
         | <Factor> "*" <Term>
         | <Factor> "/" <Term>
         | <Factor> "%" <Term>

<Factor> -> "+" <Primary> | "-" <Primary> | <Primary>

<Primary> -> <Identifier> | <Literal> | "(" <Arith_Expression> ")"
```

---

## Lexical Elements

```
<Literal> -> <Int_Literal> | <Float_Literal> | <Char_Literal> | <Bool_Literal>

<Bool_Literal> -> "TRUE" | "FALSE"

<Identifier> -> <Letter_Or_Underscore> <Alphanumeric_Or_Underscore_String>
```

---

## Operator Precedence (lowest → highest)

| Level | Operators         | Description        |
|-------|-------------------|--------------------|
| 1     | `OR`              | Logical OR         |
| 2     | `AND`             | Logical AND        |
| 3     | `NOT`             | Logical NOT        |
| 4     | `> < >= <= == <>` | Relational         |
| 5     | `+ -`             | Addition           |
| 6     | `* / %`           | Multiplication     |
| 7     | `+ -` (unary)     | Unary sign         |
| 8     | `()`              | Grouping (highest) |