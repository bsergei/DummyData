namespace RegexParser
{
    public class ParenthesisRightToken : IToken
    {
        public ParenthesisRightToken(string name, int number)
        {
            Name = name;
            Number = number;
        }

        public string Name { get; private set; }

        public int Number { get; private set; }

        public TokenType TokenType { get { return TokenType.ParenthesisRight; } }
    }
}