namespace RegexParser
{
    public class AlternationToken : IToken
    {
        public TokenType TokenType { get { return TokenType.Alternation; } }
    }
}