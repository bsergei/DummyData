namespace RegexParser
{
    public class AnyToken : IToken
    {
        public TokenType TokenType { get { return TokenType.Any; } }
    }
}