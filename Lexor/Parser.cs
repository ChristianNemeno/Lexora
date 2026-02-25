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






        // highest point , its like the entry points , the starting of the tokens,
        // we tokenized 

        public List<Stmt> Parse()
        {
            List<Stmt> statements= new List<Stmt>();

            try
            {
                // So we have tokens , consume is like , i take the current token
                // and then i see if it satisfies the requirements, really helpful
                // in closing the blocks 
                // <Declarations> -> <Declaration> <Declarations> | ε
                Consume(TokenType.Script, "Expect 'SCRIPT' at the beginning.");
                Consume(TokenType.Area, "Expect 'AREA' after 'SCRIPT'.");

                Consume(TokenType.Start, "Expect 'SCRIPT' at the beginning.");
                Consume(TokenType.Script, "Expect 'AREA' after 'SCRIPT'.");

                // declarations must be at top
                while (Match(TokenType.Declare))
                {
                    statements.Add(ParseDeclaration());
                }

                return statements;
            }
            catch (ParseError) {
                return null;
            }
        }
        public Stmt ParseDeclaration()
        {
            Token dataType;
            if(Match(TokenType.Int, TokenType.Float, TokenType.Char, TokenType.Bool))
            {
                dataType = Previous();
            }
            else
            {
                // so example , before we enter ParseDeclaration, 
                // code is like `DECLARE INT varname = 100`
                // we consumed declare then next is INT we 
                // enter this block if the token is not any of the match
                throw Error(Peek(), "Expect data type (INT, CHAR, BOOL, FLOAT).");
            }

            throw new NotImplementedException();
            // i am sleepy will cont tommrowo

        }






        // helpers

        // Current token gets ,DOES NOT MOVE
        private Token Peek()
        {
            return tokens[current];
        }
        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.Eof;
        }

        // Checks the current token in tokens , and tries to see if equal to type
        // Does not MOVE
        private bool Check(TokenType type)
        {
            if(IsAtEnd()) return false;

            return Peek().Type == type;

        }
        private Token Previous()
        {
            return tokens[current - 1];
        }
        // This MOVES
        private Token Advance()
        {
            if (!IsAtEnd()) current++;
            return Previous();
        }


        // Checks AND Moves! if Check fails , an error
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
        // its like an OR as long as the current matches any of the params
        // checks if and moves then exits the program, so likely use previous
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
