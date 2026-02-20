using System;

namespace Lexor
{
    internal enum TokenType
    {
        // Parenthesis
        LeftParen,
        RightParen,

        // Arithmetic
        Star,
        Slash,
        Mod,
        Plus,
        Minus,

        // Comparison
        Greater,
        Less,
        GreaterEqual,
        LessEqual,
        DoubleEqual,
        NotEqual,

        // Assignment / equality
        Assign,     // single '='

        // Logical
        And,
        Or,
        Not,
        True,
        False,

        // Punctuation
        Dot,

        // Data types / keywords
        Int,
        Char,
        Bool,
        Float,

        // Literals & identifiers, the actual value 
        Identifier,
        CharLiteral,
        StringLiteral,
        IntLiteral,
        FloatLiteral,

        // Symbols
        Comma,
        Ampersand,
        Dollar,

        // Reserved / script markers
        ScriptArea,
        StartScript,
        EndScript,
        Declare,

        // Control flow
        If,
        StartIf,
        EndIf,
        Else,
        ElseIf,

        // Loops
        For,
        StartFor,
        EndFor,
        RepeatWhen,
        StartRepeat,
        EndRepeat,

        Eof,
    }
}
