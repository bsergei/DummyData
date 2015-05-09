namespace RegexParser
{
    internal class CharsetLexerState : ILexerState
    {
        private bool inRange_;
        private bool isNot_;

        public IToken Handle(ILexer lexer)
        {
            switch (lexer.CurrentChar)
            {
                case '^':
                    isNot_ = true;
                    return null;

                case '\\':
                    lexer.ToState(new EscapeLexerState());
                    return null;

                case ']':
                    lexer.EndState();
                    return new BracketRightToken(isNot_);

                case '-':
                    inRange_ = true;
                    return null;

                default:
                    if (inRange_)
                    {
                        inRange_ = false;
                        return new RangeToken(lexer.CurrentChar);
                    }
                    return new LiteralToken(lexer.CurrentChar);
            }
        }
    }
}