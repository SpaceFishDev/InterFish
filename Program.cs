using System.Data.SqlTypes;

namespace InterFish
{
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
        }
    }
}