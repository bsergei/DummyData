namespace RegexParser
{
    public class ParenthesisLeftToken : IToken
    {
        public TokenType TokenType { get { return TokenType.ParenthesisLeft; } }
    }
}