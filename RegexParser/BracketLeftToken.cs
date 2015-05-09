namespace RegexParser
{
    public class BracketLeftToken : IToken 
    {
        public TokenType TokenType { get { return TokenType.BracketLeft; } }
    }
}