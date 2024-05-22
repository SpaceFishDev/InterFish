namespace InterFish
{
    class Lexer
    {
        private string Source;
        private int Pos;
        private char Current => (Pos < Source.Length) ? Source[Pos] : '\0';
        private int Line;
        private int Col;
        public Lexer(string input)
        {
            Source = input;
            Pos = 0;
        }
        private void Next()
        {
            ++Col;
            ++Pos;
        }
        public Token Lex()
        {
            if (Current == '\0')
            {
                return new Token(TokenType.Eof, "\0", Line, Col);
            }
            if (Current == '\n')
            {
                ++Line;
                Col = 0;
            }
            if (Current == ' ' || Current == '\t')
            {
                Next();
                return Lex();
            }
            if (Current == '"' | Current == '\'')
            {
                var start = Current;
                var startingColumn = Col;
                Next();
                string str = "";
                while (Current != start)
                {
                    if (Current == '\0' || Current == '\n')
                    {
                        ErrorHandler.HandleError("STR0", Line, startingColumn);
                        return new Token(TokenType.None, "", Line, startingColumn);
                    }
                    str += Current;
                    Next();
                }
                Next();
                return new Token(TokenType.String, str, Line, startingColumn);
            }
            if (Current == 'i')
            {
                Next();
                if (Current == 'f')
                {
                    Next();
                    return new Token(TokenType.If, "if", Line, Col - 2);
                }
                --Pos;
                --Col;
            }
            if (Current == 'f')
            {
                Next();
                if (Current == 'n')
                {
                    Next();
                    return new Token(TokenType.FnDecl, "fn", Line, Col - 2);
                }
                --Pos;
                --Col;
            }
            if (char.IsLetter(Current))
            {
                string str = "";
                str += Current;
                int startCol = Col;
                Next();
                while (char.IsLetterOrDigit(Current) || Current == '_')
                {
                    str += Current;
                    Next();
                }
                return new Token(TokenType.Keyword, str, Line, startCol);
            }
            if (char.IsDigit(Current))
            {
                string str = "";
                str += Current;
                int startCol = Col;
                Next();
                while (char.IsDigit(Current))
                {
                    str += Current;
                    Next();
                }
                if (Current == '.')
                {
                    str += '.';
                    Next();
                    while (char.IsDigit(Current))
                    {
                        str += Current;
                        Next();
                    }
                }
                return new Token(TokenType.Num, str, Line, startCol);
            }
            if (Current == '(')
            {
                Next();
                return new Token(TokenType.Parenthesis, "(", Line, Col - 1);
            }
            if (Current == ')')
            {
                Next();
                return new Token(TokenType.CloseParenthesis, ")", Line, Col - 1);
            }
            if (Current == '[')
            {
                Next();
                return new Token(TokenType.Bracket, "[", Line, Col - 1);
            }
            if (Current == ']')
            {
                Next();
                return new Token(TokenType.CloseBracket, "]", Line, Col - 1);
            }
            if (Current == '{')
            {
                Next();
                return new Token(TokenType.Brace, "{", Line, Col - 1);
            }
            if (Current == '}')
            {
                Next();
                return new Token(TokenType.CloseBrace, "}", Line, Col - 1);
            }

            if (Current == '!')
            {
                Next();
                if (Current == '=')
                {
                    Next();
                    return new Token(TokenType.BoolNotEq, "!=", Line, Col - 2);
                }
                return new Token(TokenType.BoolNot, "!", Line, Col - 1);
            }
            if (Current == '=')
            {
                Next();
                if (Current == '=')
                {
                    Next();
                    return new Token(TokenType.BoolEq, "==", Line, Col - 2);
                }
                return new Token(TokenType.Equals, "=", Line, Col - 1);
            }
            if (Current == '+' || Current == '-' || Current == '/' || Current == '*')
            {
                Next();
                return new Token(TokenType.Operator, Source[Pos - 1].ToString(), Line, Col - 1);
            }
            Next();
            return new Token(TokenType.None, Current.ToString(), Line, Col);
        }
    }
}