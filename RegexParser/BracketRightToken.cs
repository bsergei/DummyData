namespace RegexParser
{
    public class BracketRightToken : IToken 
    {
        private readonly bool isNegate_;

        public BracketRightToken(bool isNegate)
        {
            isNegate_ = isNegate;
        }

        public bool IsNegate
        {
            get { return isNegate_; }
        }

        public TokenType TokenType { get { return TokenType.BracketRight; } }
    }
}