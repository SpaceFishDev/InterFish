namespace InterFish
{
    struct Token
    {
        public TokenType Type;
        public string Value;
        public int Line, Col;
        public Token(TokenType type, string value, int ln, int col)
        {
            Type = type;
            Value = value;
            Line = ln;
            Col = col;
        }
        public override string ToString()
        {
            return $"Token: [{Type}, {Value}, LN:{Line}, COL:{Col}]";
        }
    }
}