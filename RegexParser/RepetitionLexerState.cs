using System;

namespace RegexParser
{
    internal class RepetitionLexerState : ILexerState
    {
        private int minOccurs_ = -1;
        private int maxOccurs_ = -1;
        private bool isParsingMinOccurs_ = true;
        private bool isParsingFirstValue_ = true;
        private int currentNumber_;

        public IToken Handle(ILexer lexer)
        {
            char character = lexer.CurrentChar;
            switch (character)
            {
                case ',':
                    if (isParsingMinOccurs_ == true)
                    {
                        if (isParsingFirstValue_ == true)
                        {
                            //missing minOccurs
                            throw new ArgumentException();
                        }
                        else
                        {
                            //end
                            minOccurs_ = currentNumber_;
                            currentNumber_ = 0;
                            isParsingMinOccurs_ = false;
                            isParsingFirstValue_ = true;
                        }
                    }
                    else
                    {
                        //too many ,
                        throw new ArgumentException();
                    }
                    break;
                case '}':
                    if (isParsingMinOccurs_ == true)
                    {
                        if (isParsingFirstValue_ == true)
                        {
                            //missing minOccurs
                            throw new ArgumentException();
                        }
                        else
                        {
                            //minOccurs equals maxOccurs
                            minOccurs_ = currentNumber_;
                            maxOccurs_ = currentNumber_;
                        }
                    }
                    else
                    {
                        if (isParsingFirstValue_ == true)
                        {
                            //maxOccus = unlimited
                            maxOccurs_ = -1;
                        }
                        else
                        {
                            //end
                            maxOccurs_ = currentNumber_;
                        }
                    }
                    lexer.EndState();
                    return new RepetitionToken(minOccurs_, maxOccurs_);
                default:
                    currentNumber_ = currentNumber_ * 10 + int.Parse(character.ToString()); //test?
                    isParsingFirstValue_ = false;
                    break;
            }
            return null;
        }
    }
}