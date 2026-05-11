# Language Specification of LEXOR Programming Language

---

## Introduction

LEXOR is a **strongly-typed programming language** developed to teach Senior High School students the basics of programming. It was developed by a group of students enrolled in the Programming Languages course. LEXOR is a **pure interpreter**.

---

## Sample Program

```
%% this is a sample program in LEXOR
SCRIPT AREA
START SCRIPT
    DECLARE INT x, y, z=5
    DECLARE CHAR a_1='n'
    DECLARE BOOL t="TRUE"
    x=y=4
    a_1='c'
    %% this is a comment
    PRINT: x & t & z & $ & a_1 & [#] & "last"
END SCRIPT
```

**Output:**
```
4TRUE5
c#last
```

---

## Language Grammar

### Program Structure

- All code starts with `SCRIPT AREA`
- All code is placed inside `START SCRIPT` and `END SCRIPT`
- All variable declarations must follow right after the `START SCRIPT` keyword — they **cannot** be placed anywhere else
- All variable names are **case-sensitive** and must start with a letter or an underscore (`_`), followed by letters, underscores, or digits
- Every line contains a **single statement**
- Comments start with a double percent sign (`%%`) and can be placed anywhere in the program
- Executable code is placed **after** variable declarations
- All **reserved words** are in capital letters and cannot be used as variable names
- The dollar sign (`$`) signifies a **next line / carriage return**
- The ampersand (`&`) serves as a **concatenator**
- Square brackets (`[]`) are used as **escape codes**

---

## Data Types

| Type    | Description                                      | Size    |
|---------|--------------------------------------------------|---------|
| `INT`   | An ordinary number with no decimal part          | 4 bytes |
| `CHAR`  | A single symbol                                  | —       |
| `BOOL`  | Represents the literals `TRUE` or `FALSE`        | —       |
| `FLOAT` | A number with a decimal part                     | 4 bytes |

---

## Operators

### Arithmetic Operators (in order of precedence)

| Operator       | Description                              |
|----------------|------------------------------------------|
| `( )`          | Parenthesis (grouping)                   |
| `*`, `/`, `%`  | Multiplication, Division, Modulo         |
| `+`, `-`       | Addition, Subtraction                    |
| `>`, `<`       | Greater than, Lesser than               |
| `>=`, `<=`     | Greater than or equal, Lesser than or equal |
| `==`, `<>`     | Equal, Not equal                         |

### Logical Operators

Syntax: `<BOOL expression> <LogicalOperator> <BOOL expression>`

| Operator | Description                                                                 |
|----------|-----------------------------------------------------------------------------|
| `AND`    | Returns `TRUE` only if both BOOL expressions are `TRUE`, otherwise `FALSE`  |
| `OR`     | Returns `TRUE` if at least one BOOL expression is `TRUE`, otherwise `FALSE` |
| `NOT`    | Returns the reverse/negated value of the BOOL expression                    |

### Unary Operators

| Operator | Description |
|----------|-------------|
| `+`      | Positive    |
| `-`      | Negative    |

---

## Input / Output Statements

### Output — `PRINT`

Writes formatted output to the output device.

**Syntax:**
```
PRINT: <expression> [& <expression>]*
```

**Special tokens in PRINT:**
- `&` — concatenator (joins values together)
- `$` — newline / carriage return
- `[<char>]` — escape code (e.g., `[#]` prints `#`, `[[]` prints `[`)

**Example:**
```
PRINT: x & t & z & $ & a_1 & [#] & "last"
```

---

### Input — `SCAN`

Allows the user to input a value to a variable.

**Syntax:**
```
SCAN: <variableName> [, <variableName>]*
```

**Example:**
```
SCAN: x, y
```
> The user must input two values separated by a comma (`,`).

---

## Variable Declaration

**Syntax:**
```
DECLARE <DataType> <varName> [= <value>] [, <varName> [= <value>]]*
```

**Examples:**
```
DECLARE INT x, y, z=5
DECLARE CHAR a_1='n'
DECLARE BOOL t="TRUE"
DECLARE INT abc=100
```

- `CHAR` literals are enclosed in **single quotes** (`'n'`)
- `BOOL` literals are enclosed in **double quotes** (`"TRUE"` or `"FALSE"`)
- Multiple variables of the same type can be declared in a single statement, separated by commas

---

## Control Flow Structures

### 1. Conditional Statements

#### a. `IF` Selection

```
IF (<BOOL expression>)
START IF
    <statement>
    ...
    <statement>
END IF
```

#### b. `IF-ELSE` Selection

```
IF (<BOOL expression>)
START IF
    <statement>
    ...
    <statement>
END IF
ELSE
START IF
    <statement>
    ...
    <statement>
END IF
```

#### c. `IF-ELSE IF-ELSE` (Multiple Alternatives)

```
IF (<BOOL expression>)
START IF
    <statement>
    ...
    <statement>
END IF
ELSE IF (<BOOL expression>)
START IF
    <statement>
    ...
    <statement>
END IF
ELSE
START IF
    <statement>
    ...
    <statement>
END IF
```

---

### 2. Loop Control Flow Structures

#### a. `FOR` Loop

```
FOR (<initialization>, <condition>, <update>)
START FOR
    <statement>
    ...
    <statement>
END FOR
```

#### b. `REPEAT WHEN` Loop (While-like)

```
REPEAT WHEN (<BOOL expression>)
START REPEAT
    <statement>
    ...
    <statement>
END REPEAT
```

---

## Sample Programs

### Program 1 — Arithmetic Operation

```
SCRIPT AREA
START SCRIPT
    DECLARE INT xyz, abc=100
    xyz= ((abc *5)/10 + 10) * -1
    PRINT: [[] & xyz & []]
END SCRIPT
```

**Output:**
```
[-60]
```

> `[[]` is an escape code that prints the literal `[` character.

---

### Program 2 — Logical Operation

```
SCRIPT AREA
START SCRIPT
    DECLARE INT a=100, b=200, c=300
    DECLARE BOOL d="FALSE"
    d = (a < b AND c <>200)
    PRINT: d
END SCRIPT
```

**Output:**
```
TRUE
```

---

## Implementation Notes

> **Note:** You may use any programming language to implement the LEXOR interpreter **except Python and JavaScript.**

---

## Reserved Words

| Reserved Word  | Purpose                              |
|----------------|--------------------------------------|
| `SCRIPT AREA`  | Marks the beginning of the program   |
| `START SCRIPT` | Begins the executable block          |
| `END SCRIPT`   | Ends the executable block            |
| `DECLARE`      | Variable declaration keyword         |
| `INT`          | Integer data type                    |
| `CHAR`         | Character data type                  |
| `BOOL`         | Boolean data type                    |
| `FLOAT`        | Float data type                      |
| `PRINT`        | Output statement                     |
| `SCAN`         | Input statement                      |
| `IF`           | Conditional statement                |
| `ELSE`         | Alternative branch                   |
| `ELSE IF`      | Additional conditional branch        |
| `START IF`     | Opens a conditional block            |
| `END IF`       | Closes a conditional block           |
| `FOR`          | For loop                             |
| `START FOR`    | Opens a for loop block               |
| `END FOR`      | Closes a for loop block              |
| `REPEAT WHEN`  | While-like loop                      |
| `START REPEAT` | Opens a repeat loop block            |
| `END REPEAT`   | Closes a repeat loop block           |
| `AND`          | Logical AND operator                 |
| `OR`           | Logical OR operator                  |
| `NOT`          | Logical NOT operator                 |
| `TRUE`         | Boolean literal                      |
| `FALSE`        | Boolean literal                      |