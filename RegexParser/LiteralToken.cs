namespace RegexParser
{
    public class LiteralToken : IToken
    {
        public LiteralToken(char character)
        {
            Character = character;
        }

        public char Character { get; private set; }
        public TokenType TokenType { get { return TokenType.Literal; } }
    }
}