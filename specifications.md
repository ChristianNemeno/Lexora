Language Specification of LEXOR Programming Language

Introduction

LEXOR is a strongly typed programming language developed to teach Senior High School students the basics of programming. It was developed by a group of students enrolled in the Programming Languages course. LEXOR is a pure interpreter.

Sample Program

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


Output of the sample program:

4TRUE5
c#last


Language Grammar

Program Structure:

All codes start with SCRIPT AREA.

All codes are placed inside START SCRIPT and END SCRIPT.

All variable declarations follow right after the START SCRIPT keyword. It cannot be placed anywhere else.

All variable names are case sensitive and start with a letter or an underscore (_) and followed by a letter, underscore, or digits.

Every line contains a single statement.

Comments start with a double percent sign (%%) and can be placed anywhere in the program.

Executable codes are placed after variable declarations.

All reserved words are in capital letters and cannot be used as variable names.

Dollar sign ($) signifies next line or carriage return.

Ampersand (&) serves as a concatenator.

The square braces ([]) act as escape codes.

Data Types:

INT - An ordinary number with no decimal part. It occupies 4 bytes in the memory.

CHAR - A single symbol.

BOOL - Represents the literals "TRUE" or "FALSE".

FLOAT - A number with a decimal part. It occupies 4 bytes in the memory.

Operators:

Arithmetic operators:

() - parenthesis

*, /, % - multiplication, division, modulo

+, - - addition, subtraction

>, < - greater than, lesser than

>=, <= - greater than or equal to, lesser than or equal to

==, <> - equal, not equal

Logical operators: (Syntax: <BOOL expression> <LogicalOperator> <BOOL expression>)

AND - needs the two BOOL expressions to be true to result to true, else false.

OR - if one of the BOOL expressions evaluates to true, returns true, else false.

NOT - the reverse value of the BOOL value.

Unary operators:

+ - positive

- - negative

Sample Programs

1. A program with arithmetic operation

SCRIPT AREA
START SCRIPT
DECLARE INT xyz, abc=100
xyz=((abc*5)/10+10)*-1
PRINT: [[] & xyz & []]
END SCRIPT


Output:

[-60]


2. A program with logical operation

SCRIPT AREA
START SCRIPT
DECLARE INT a=100, b=200, c=300
DECLARE BOOL d="FALSE"
d=(a<b AND c<>200)
PRINT: d
END SCRIPT


Output:

TRUE


Input / Output Statements

Output statement:

PRINT - writes formatted output to the output device.

Input statement:

SCAN - allow the user to input a value to a data type.

Syntax: SCAN: <variableName>[,<variableName>]*

Sample use: SCAN: x, y

Note: This means in the screen you have to input two values separated by comma (,).

Control Flow Structures

1. Conditional

a. if selection

IF (<BOOL expression>)
START IF
    <statement>
    <statement>
END IF


b. if-else selection

IF (<BOOL expression>)
START IF
    <statement>
    <statement>
END IF
ELSE
START IF
    <statement>
    <statement>
END IF


c. if-else with multiple alternatives

IF (<BOOL expression>)
START IF
    <statement>
    <statement>
END IF
ELSE IF (<BOOL expression>)
START IF
    <statement>
    <statement>
END IF
ELSE
START IF
    <statement>
    <statement>
END IF


2. Loop Control Flow Structures

a. FOR (initialization, condition, update)

START FOR
    <statement>
    <statement>
END FOR


b. REPEAT WHEN ()

START REPEAT
    <statement>
    <statement>
END REPEAT


Note: You may use any language to implement the interpreter except Python and Javascript.