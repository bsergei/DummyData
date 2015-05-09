namespace RegexParser
{
    internal class LiteralLexerState : ILexerState
    {
        private readonly CommonState commonState_;

        public LiteralLexerState(CommonState commonState)
        {
            commonState_ = commonState;
        }

        public IToken Handle(ILexer lexer)
        {
            switch (lexer.CurrentChar)
            {
                case '(':
                    lexer.ToState(new ParenthesisLexerState(commonState_));
                    return new ParenthesisLeftToken();
                case '{':
                    lexer.ToState(new RepetitionLexerState());
                    break;
                case '\\':
                    lexer.ToState(new EscapeLexerState());
                    return null;
                case '[':
                    lexer.ToState(new CharsetLexerState());
                    return new BracketLeftToken();
                case '*':
                    return new RepetitionToken(0, null);
                case '?':
                    return new RepetitionToken(0, 1);
                case '+':
                    return new RepetitionToken(1, null);
                case '|':
                    return new AlternationToken();
                case '.':
                    return new AnyToken();
                default:
                    return new LiteralToken(lexer.CurrentChar);
            }
            return null;
        }
    }
}