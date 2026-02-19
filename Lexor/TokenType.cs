using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexor
{
    internal enum TokenType
    {
        // Parenthesis (, )
        LEFT_PAREN,
        RIGHT_PAREN,
        // * , / , %
        STAR, SLASH, MOD,
        // +, -
        PLUS, MINUS,
        // > , < , <=, >= , ==, <>
        GREATER, LESS,
        GREATER_EQUAL, LESS_EQUAL,
        DOUBLE_EQUAL, NOT_EQUAL,

        // =    
        EQUAL,


        // logical ops
        AND, OR, NOT, TRUE, FALSE,

        //.
        DOT,

        // data types
        INT, CHAR, BOOL, FLOAT,

        // literals
        IDENTIFIER, CHAR_LITERAL, STRING_LITERAL, INT_LITERAL, FLOAT_LITERAL,

        //symbols
        COMMA, AMPERSAND, DOLLAR,

        // reserved keywords
        SCRIPT_AREA, START_SCRIPT, END_SCRIPT,
        DECLARE,

        // the '=' button
        ASSIGN,

        // control flow
        IF, START_IF, END_IF,
        ELSE, ELSE_IF,

        //loop
        FOR, START_FOR, END_FOR,
        REPEAT_WHEN, START_REPEAT, END_REPEAT,


        EOF,
    }
}
