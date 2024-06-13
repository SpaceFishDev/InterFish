


namespace InterFish
{
    enum NodeType
    {
        None,
        Program,
        Expression,
        Literal,
        BinaryExpression,
        If,
        Call,
        Function,
    }
    class Node
    {
        public NodeType nodeType;
        public Token nToken;
        public List<Node?> Children;
        public Node(NodeType nodeType, Token token)
        {
            this.nodeType = nodeType;
            this.nToken = token;
            this.Children = new();
        }
        private string Str(int indent)
        {
            string output = $"N: [{nodeType}; {nToken.Value}]\n";
            for (int i = 0; i < indent; ++i)
            {
                output = "--->" + output;
            }
            ++indent;
            foreach (var child in Children)
            {
                output += child.Str(indent);
            }
            return output;
        }
        public override string ToString()
        {
            return Str(0);
        }
    }
    class Parser
    {
        private List<Token> Tokens = new();
        private int Pos = 0;
        public bool Expect(TokenType T, bool x)
        {
            if (Tokens[Pos + 1].Type == T)
            {
                return true;
            }
            if (x)
            {
                ErrorHandler.HandleError($"Unexpected Token ['{Tokens[Pos + 1].Value}'] of type [{Tokens[Pos + 1].Type}] ", Tokens[Pos + 1].Line, Tokens[Pos + 1].Col);
            }
            return false;
        }
        public void Next()
        {
            ++Pos;
        }

        private Token Current => (Pos < Tokens.Count) ? Tokens[Pos] : new(TokenType.Eof, "", 0, 0);
        public Node Root;
        public Parser(List<Token> tokens)
        {
            Tokens = tokens;
            Root = new(NodeType.Program, new(TokenType.None, "", 0, 0));
        }
        public Node ParseBasic()
        {
            switch (Current.Type)
            {
                case TokenType.String:
                    return new(NodeType.Literal, Current);
                case TokenType.Num:
                    return new(NodeType.Literal, Current);
            }
            return new(NodeType.Literal, Current);
        }
        public Node ParsePrimary()
        {
            Node left = ParseBasic();
            Next();
            Node n = new(NodeType.BinaryExpression, Current);
            --Pos;
            bool isBinExpr = false;
            Node? right = null;
            if (Expect(TokenType.Operator, false))
            {
                isBinExpr = true;
                Next();
                Next();
                right = ParsePrimary();
            }
            if (isBinExpr)
            {
                n.Children.Add(left);
                n.Children.Add(right);
                return n;
            }
            return left;
        }
        public Node ParseFactor()
        {
            Node left = ParsePrimary();
            Next();
            Node n = new(NodeType.BinaryExpression, Current);
            --Pos;
            bool isBinExpr = false;
            Node? right = null;
            if (Expect(TokenType.Operator, false))
            {
                isBinExpr = true;
                right = ParseFactor();
            }
            if (isBinExpr)
            {
                n.Children.Add(left);
                n.Children.Add(right);
                return n;
            }
            return left;
        }
        public Node ParseBinExpr()
        {
            Node n = ParseFactor();
            return n;
        }
        public Node Parse(Node Parent)
        {
            if (Expect(TokenType.Operator, false))
            {
                Node Expr = new(NodeType.Expression, new(TokenType.None, "", 0, 0));
                Node n = ParseBinExpr();
                Expr.Children.Add(n);
                Parent.Children.Add(Expr);
                return Parent;
            }
            if (Current.Type == TokenType.FnDecl)
            {
                if (Parent.nodeType == NodeType.Program)
                {
                    Expect(TokenType.Keyword, true);
                    Next();
                    Node Fn = new(NodeType.Function, Current);
                    Root.Children.Add(Parse(Fn));
                }
                else
                {
                    Expect(TokenType.Keyword, true);
                    Next();
                    Node Fn = new(NodeType.Function, Current);
                    Parent.Children.Add(Fn);
                }
            }
            return Parent;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string TestProgram = File.ReadAllText("test.if");
            Lexer lexer = new(TestProgram);
            List<Token> tokens = new();
            Token tok = new(TokenType.None, "", 0, 0);
            while (tok.Type != TokenType.Eof)
            {
                tok = lexer.Lex();
                tokens.Add(tok);
            }
            foreach (Token t in tokens)
            {
                if (t.Type != TokenType.None)
                {
                    Console.WriteLine(t.ToString());
                }
            }
            foreach (var err in ErrorHandler.Errors)
            {
                Console.WriteLine(err.ToString());
            }

            Parser p = new(tokens);
            p.Parse(p.Root);
            Console.WriteLine(p.Root);
        }
    }
}