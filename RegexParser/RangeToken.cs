namespace RegexParser
{
    public class RangeToken : IToken
    {
        private readonly char endChar_;

        public RangeToken(char endChar)
        {
            endChar_ = endChar;
        }

        public TokenType TokenType { get { return TokenType.Range; } }

        public char EndChar
        {
            get { return endChar_; }
        }
    }
}