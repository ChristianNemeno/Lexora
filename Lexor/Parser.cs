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
                /// So we have tokens , consume is like , i take the current token
                /// and then i see if it satisfies the requirements, really helpful
                /// in closing the blocks 
                /// <Declarations> -> <Declaration> <Declarations> | ε
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

            // naka consume natas DECLARE token 
            Token dataType;
            if(Match(TokenType.Int, TokenType.Float, TokenType.Char, TokenType.Bool))
            {
                dataType = Previous();
            }
            else
            {
                /// so example , before we enter ParseDeclaration, 
                /// code is like `DECLARE INT varname = 100`
                /// we consumed declare then next is INT we 
                /// enter this block if the token is not any of the match
                throw Error(Peek(), "Expect data type (INT, CHAR, BOOL, FLOAT).");
            }

            List<Stmt.Declarator> variables = new List<Stmt.Declarator>();

            do
            {
                Token name = Consume(TokenType.Identifier, "Expects variable name");
                Expr initializer = null;

                if (Match(TokenType.Assign))
                {
                    initializer = Expression();
                }

                variables.Add(new Stmt.Declarator(name, initializer));


            } while (Match(TokenType.Comma));
            
            // after declarations we can procced with other statements but i will implement Expression first  
            
            throw new NotImplementedException();
            
        }



        // Expressions are different to statements, theyre like something of value
        // vs sa Stmt, they do something, so like in arithmetic there is a precedence

        private Expr Expression()
        {
            return Assignment();
        }

        private Expr Assignment()
        {
            Expr expr = LogicalOr(); // ypu can see why this is sa later recursion tree

            if (Match(TokenType.Assign){
                Token equals = Previous();
                Expr value = Assignment();

                if(expr is Expr.Variable variable)
                {
                    Token Name = variable.Name;
                    return new Expr.Assign(Name, value);
                }
                Error(equals, "Invalid assignment target.");
            }
            return expr;
        }
        private Expr LogicalOr()
        {
            Expr expr = LogicalAnd();

            while (Match(TokenType.Or))
            {
                Token op = Previous();
                Expr right = LogicalAnd();
                expr = new Expr.Binary(expr, op, right);
            }
            return expr;
        }

        private Expr LogicalAnd()
        {
            Expr expr = ParseEquality();

            while (Match(TokenType.And))
            {
                Token op = Previous();
                Expr right = ParseEquality();
                expr = new Expr.Binary(expr, op, right);
            }
            return expr;
        }

        private Expr Equality()
        {
            Expr expr = ParseComparison();

            while (Match(TokenType.NotEqual, TokenType.DoubleEqual))
            {
                Token op = Previous();
                Expr right = ParseComparison();
                expr = new Expr.Binary(expr, op, right);
            }
            return expr;
        }

        private Expr Comparison()
        {
            Expr expr = ParseTerm();

            while (Match(TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual))
            {
                Token op = Previous();
                Expr right = ParseTerm();
                expr = new Expr.Binary(expr, op, right);
            }
            return expr;
        }

        private Expr Term()
        {
            Expr expr = ParseFactor();

            while (Match(TokenType.Minus, TokenType.Plus))
            {
                Token op = Previous();
                Expr right = ParseFactor();
                expr = new Expr.Binary(expr, op, right);
            }
            return expr;
        }

        private Expr Factor()
        {
            Expr expr = ParseUnary();

            while (Match(TokenType.Slash, TokenType.Star, TokenType.Mod))
            {
                Token op = Previous();
                Expr right = ParseUnary();
                expr = new Expr.Binary(expr, op, right);
            }
            return expr;
        }

        private Expr Unary()
        {
            if (Match(TokenType.Not, TokenType.Minus, TokenType.Plus))
            {
                Token op = Previous();
                Expr right = ParseUnary();
                return new Expr.Unary(op, right);
            }
            return ParsePrimary();
        }

        private Expr Primary()
        {
            if (Match(TokenType.BoolLiteral))
            {
                return new Expr.Literal(Previous().Literal);
            }

            if (Match(TokenType.IntLiteral, TokenType.FloatLiteral, TokenType.CharLiteral, TokenType.StringLiteral))
            {
                return new Expr.Literal(Previous().Literal);
            }

            if (Match(TokenType.Dollar))
            {
                return new Expr.Literal("\n");
            }

            if (Match(TokenType.LeftBracket))
            {
                Expr inner = Expression();
                Consume(TokenType.RightBracket, "Expect ']' after escape code.");
                return inner;
            }

            if (Match(TokenType.Identifier))
            {
                return new Expr.Variable(Previous());
            }

            if (Match(TokenType.LeftParen))
            {
                Expr expr = Expression();
                Consume(TokenType.RightParen, "Expect ')' after expression.");
                return new Expr.Grouping(expr);
            }

            throw Error(Peek(), "Expect expression.");
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
