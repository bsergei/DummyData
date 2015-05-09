using System.Collections.Generic;

namespace RegexParser
{
    public class RegexLexer : ILexer
    {
        Stack<ILexerState> states_;
        ILexerState currentState_;
        IEnumerator<char> characters_;

        public IEnumerable<IToken> Tokenize(string expression)
        {
            states_ = new Stack<ILexerState>();
            currentState_ = new LiteralLexerState(new CommonState());
            characters_ = expression.GetEnumerator();
            while (characters_.MoveNext())
            {
                IToken token = currentState_.Handle(this);
                if (token != null)
                {
                    yield return token;
                }
            }
        }

        public void ToState(ILexerState state)
        {
            states_.Push(currentState_);
            currentState_ = state;
        }

        public void EndState()
        {
            currentState_ = states_.Pop();
        }

        public char CurrentChar
        {
            get
            {
                return characters_.Current;
            }
        }
    }
}