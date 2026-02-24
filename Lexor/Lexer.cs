using System;
using System.Collections.Generic;

namespace Lexor;

internal class Lexer
{
    private readonly string _source;
    private readonly List<Token> _tokens = new();

    private int _start;
    private int _current;
    private int _line = 1;

    private static readonly Dictionary<string, TokenType> Keywords;

    static Lexer()
    {
        Keywords = new Dictionary<string, TokenType>(StringComparer.Ordinal)
        {
            // Keywords
            ["SCRIPT"] = TokenType.Script,
            ["AREA"] = TokenType.Area,
            ["START"] = TokenType.Start,
            ["END"] = TokenType.End,
            ["DECLARE"] = TokenType.Declare,
            ["PRINT"] = TokenType.Print,
            ["SCAN"] = TokenType.Scan,


            // Loops , Controlflow
            ["IF"] = TokenType.If,
            ["ELSE"] = TokenType.Else,
            ["FOR"] = TokenType.For,
            ["REPEAT"] = TokenType.Repeat,
            ["WHEN"] = TokenType.When,

            // Data types
            ["INT"] = TokenType.Int,
            ["CHAR"] = TokenType.Char,
            ["BOOL"] = TokenType.Bool,
            ["FLOAT"] = TokenType.Float,

            //Logic
            ["TRUE"] = TokenType.True,
            ["FALSE"] = TokenType.False,
            ["AND"] = TokenType.And,
            ["OR"] = TokenType.Or,
            ["NOT"] = TokenType.Not,

        };
    }

    public Lexer(string source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
    }

   

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            _start = _current;
            ScanToken();
        }

        _tokens.Add(new Token(TokenType.Eof, "", null, _line));
        return _tokens;
    }

    // Scanning part 

    private void ScanToken()
    {
        char ch = Advance();

        switch (ch)
        {
            case '(': AddToken(TokenType.LeftParen);  break;
            case ')': AddToken(TokenType.RightParen); break;
            case ',': AddToken(TokenType.Comma);      break;
            case '.': AddToken(TokenType.Dot);        break;
            case '-': AddToken(TokenType.Minus);      break;
            case '+': AddToken(TokenType.Plus);       break;
            case '*': AddToken(TokenType.Star);       break;
            case '/': AddToken(TokenType.Slash);      break;
            case ':': AddToken(TokenType.Colon);      break;
            case '&': AddToken(TokenType.Ampersand);  break;

            case '=': AddToken(Match('=') ? TokenType.DoubleEqual : TokenType.Assign); break;
            case '<': AddToken(Match('=') ? TokenType.LessEqual    : Match('>') ? TokenType.NotEqual : TokenType.Less);    break;
            case '>': AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater); break;
            case '$': AddToken(TokenType.Dollar); break;

            case '[': AddToken(TokenType.LeftBracket); break;
            case ']': AddToken(TokenType.RightBracket); break;
            // Whitespace — ignore
            case ' ':
            case '\r':
            case '\t':
                break;


            case '\'': CharLiteral(); break;
            case '"': StringLiteral(); break;
            case '\n':
                _line++;
                break;

            // '%' = modulus, or '%%' = line comment
            case '%':
                if (Match('%'))
                    while (Peek() != '\n' && !IsAtEnd()) Advance();
                else
                    AddToken(TokenType.Mod);
                break;

            default:
                if (IsDigit(ch))
                {
                    Number();
                }else if (IsAlpha(ch))
                {

                    Identifier();
                }
                else
                {
                    // error here
                    Lexora.Error(_line, "Unexpected character");
                }
                break;
        }
    }
    private void CharLiteral()
    {
        if (IsAtEnd() || PeekNext() != '\'')
        {
            // its things like this '' , Im not sure if thats invalid or not 
            Lexora.Error(_line, "Invalid char literal");
            return;
        }
        char value = Advance();
        Advance();
        AddToken(TokenType.CharLiteral, value);

    }
    private void StringLiteral()
    {
        while (Peek() != '"' && !IsAtEnd()) {

            if (Peek() == '\n') _line++;
            Advance();
        }

        if (IsAtEnd())
        {
            Lexora.Error(_line, "Unterminated String");
            return ;
        }

        Advance(); // consume   

        string value = _source.Substring(_start + 1, _current - _start - 2);

        if( value == "TRUE")
        {
            AddToken(TokenType.BoolLiteral, true);
        }
        else if( value == "FALSE")
        {
            AddToken(TokenType.BoolLiteral, false);
        }
        else
        {
            AddToken(TokenType.StringLiteral, value);
        }



    }

    private void Identifier()
    {
        while (IsAlphaNumeric(Peek())){
            Advance();
        }

        string text = _source.Substring(_start, _current - _start);

        TokenType type;
        if (!Keywords.TryGetValue(text, out type))
        {
            type = TokenType.Identifier; 
        }

        AddToken(type);
        
    }
    private void Number()
    {
        bool isFloat = false;


        while(IsDigit(Peek()))
        {
            Advance();
        }

        if(Peek() == '.' && IsDigit(PeekNext()))
        {
            isFloat = true;
            Advance(); // because we want to ignore the '.' else bottom code would early exit
            while (IsDigit(Peek()))
            {
                Advance();
            }
        }

        string value = _source.Substring(_start, _current - _start);

        if (isFloat)
            AddToken(TokenType.FloatLiteral, float.Parse(value));
        else
            AddToken(TokenType.IntLiteral, int.Parse(value));

    }


    private bool IsDigit(char ch)
    {
        return ch >= '0' && ch <= '9';
    }
    private bool IsAlpha(char c)
    {
        return (c >= 'a' && c <= 'z') ||
           (c >= 'A' && c <= 'Z') ||
           c == '_';
    }

    private bool IsAlphaNumeric(char c)
    {
        return IsDigit(c) | IsAlpha(c);

    }
    


    private void AddToken(TokenType type) => AddToken(type, null);

    private void AddToken(TokenType type, Object? literal)
    {
        string lexeme = _source.Substring(_start, _current - _start);
        _tokens.Add(new Token(type, lexeme, literal, _line));
    }


    //Consumes and returns the current character.
    private char Advance() => _source[_current++];

    // Returns true if the current character matches and advances
    private bool Match(char expected)
    {
        if (IsAtEnd()) return false;
        if (_source[_current] != expected) return false;
        _current++;
        return true;
    }

    // Peeks at the current character without consuming it
    private char Peek() => IsAtEnd() ? '\0' : _source[_current];

    // Peeks at the next character without consuming it
    private char PeekNext() => _current + 1 >= _source.Length ? '\0' : _source[_current + 1];

    // Returns true when all source characters have been consumed
    public bool IsAtEnd() => _current >= _source.Length;
}
