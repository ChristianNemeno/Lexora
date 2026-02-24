namespace Lexor
{
    internal enum TokenType
    {
        // Parenthesis & Brackets
        LeftParen,
        RightParen,
        LeftBracket,
        RightBracket,

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

        // Assignment
        Assign,

        // Logical
        And,
        Or,
        Not,

        // Punctuation & Symbols
        Dot,
        Comma,
        Colon,
        Ampersand,
        Dollar,

        // Data types 
        Int,
        Char,
        Bool,
        Float,

        // Literals & Identifiers
        Identifier,
        CharLiteral,
        StringLiteral,
        IntLiteral,
        FloatLiteral,
        BoolLiteral,

        // Keywords (Strictly Single Words)
        Script,
        Area,
        Start,
        End,
        Declare,
        Print,
        Scan,
        If,
        Else,
        For,
        Repeat,
        When,
        True,
        False,

        Eof
    }
}