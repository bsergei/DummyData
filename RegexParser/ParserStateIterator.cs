using System;
using System.Collections.Generic;

namespace RegexParser
{
    class ParserStateIterator
    {
        private Stack<ParserState> states_;
        private ParserState currentState_;
        private IEnumerator<IToken> tokens_;
        private HashSet<ISyntax> processedSyntaxes_;

        public ISyntax Parse(string expression)
        {
            processedSyntaxes_ = new HashSet<ISyntax>();

            states_ = new Stack<ParserState>();
            currentState_ = new ParserState();
            tokens_ = new RegexLexer().Tokenize(expression).GetEnumerator();
            while (tokens_.MoveNext())
            {
                currentState_.Handle(this);
            }

            while (states_.Count > 0)
            {
                EndState();
            }

            currentState_.AddSyntax(new ParenthesisSyntax(new ParenthesisRightToken(String.Empty, 0)));
            return currentState_.Process(processedSyntaxes_);
        }

        public void ToState(ParserState state)
        {
            states_.Push(currentState_);
            currentState_ = state;
        }

        public ParserState EndState()
        {
            ParserState toState = states_.Pop();
            toState.AddSyntax(currentState_.Process(processedSyntaxes_));
            currentState_ = toState;
            return currentState_;
        }

        public IToken Current
        {
            get
            {
                return tokens_.Current;
            }
        }
    }
}