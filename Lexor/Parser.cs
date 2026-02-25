using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexor
{
    class Parser
    {
        // Sentinel exception used to unwind the parser on a syntax error
        private class ParseError : Exception { }

        private readonly List<Token> tokens;
        private int current = 0;
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }













        // helpers
        private Token Peek()
        {
            return tokens[current];
        }
        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.Eof;
        }
        private bool Check(TokenType type)
        {
            if(IsAtEnd()) return false;

            return Peek().Type == type;

        }
        private Token Previous()
        {
            return tokens[current - 1];
        }

        private Token Advance()
        {
            if (!IsAtEnd()) current++;
            return Previous();
        }
        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
                return Advance();

            throw Error(Previous(), message);
        }

        // Reports the error and returns a ParseError to be thrown by the caller
        private static ParseError Error(Token token, string message)
        {
            Lexora.Error(token, message);
            return new ParseError();
        }
        private bool Match(params TokenType[] types)
        {
            foreach (TokenType t in types)
            {
                if (Check(t))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }



    }
}
