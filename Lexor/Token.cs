using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexor
{
    internal class Token
    {
        public TokenType Type { get; }
        public String? Lexeme { get; }
        public Object? Literal { get; }
        public int Line { get; }

        public Token(TokenType type, String? lexeme, Object? literal, int line)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;
        }

        public override String ToString() => $"{Type} {Lexeme} {Literal ?? "null"}";




    }
}
