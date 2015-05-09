using System.Text;

namespace RegexParser
{
    internal class ParenthesisLexerState : ILexerState
    {
        private readonly CommonState commonState_;
        private readonly LiteralLexerState literalLexerState_;
        private readonly StringBuilder groupNameBuilder_ = new StringBuilder();

        private bool groupCaptureStarted_;
        private bool inNameStarted_;
        private bool inNameFinished_;
        private bool isParsingFirstValue_ = true;

        public ParenthesisLexerState(CommonState commonState)
        {
            commonState_ = commonState;
            literalLexerState_ = new LiteralLexerState(commonState_);
            inNameStarted_ = true;
        }

        public IToken Handle(ILexer lexer)
        {
            char character = lexer.CurrentChar;
            try
            {
                switch (character)
                {
                    case '?':
                        if (isParsingFirstValue_) // first char after (.
                        {
                            groupCaptureStarted_ = true;
                            return null;
                        }
                        else
                        {
                            goto default;
                        }

                    case ':':
                        if (groupCaptureStarted_)
                        {
                            groupCaptureStarted_ = false;
                            return null;
                        }
                        else
                        {
                            goto default;
                        }

                    case '<':
                        if (groupCaptureStarted_)
                        {
                            inNameStarted_ = true;
                            return null;
                        }
                        else
                        {
                            goto default;
                        }

                    case '>':
                        if (groupCaptureStarted_ && inNameStarted_)
                        {
                            inNameFinished_ = true;
                            return null;
                        }
                        else
                        {
                            goto default;
                        }

                    case ')':
                        lexer.EndState();
                        var groupNumber = commonState_.GroupNumber;
                        commonState_.GroupNumber++;
                        return new ParenthesisRightToken(groupNameBuilder_.ToString(), groupNumber);

                    default:
                        if (groupCaptureStarted_ && inNameStarted_ && !inNameFinished_)
                        {
                            groupNameBuilder_.Append(lexer.CurrentChar);
                            return null;
                        }

                        return literalLexerState_.Handle(lexer);
                }
            }
            finally
            {
                isParsingFirstValue_ = false;
            }
        }
    }
}