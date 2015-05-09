using System;

namespace RegexParser
{
    internal class EscapeLexerState : ILexerState
    {
        public IToken Handle(ILexer lexer)
        {
            switch (lexer.CurrentChar)
            {
                case 'a':
                    //case 'b':
                case 't':
                case 'r':
                case 'v':
                case 'f':
                case 'n':
                case 'e':
                    //case nnn: Uses octal representation to specify a character (nnn consists of two or three digits).
                    //case nn: Uses hexadecimal representation to specify a character (nn consists of exactly two digits).

                case 'c': // Matches the ASCII control character that is specified by X or x, where X or x is the letter of the control character. 
                case 'u': // Matches a Unicode character by using hexadecimal representation (exactly four digits, as represented by nnnn).

                    // Anchors
                case 'A':
                case 'Z':
                case 'z':
                case 'G':
                case 'b':
                case 'B':

                case 'P': // Single char in unicode category
                case 'p': // Single char in unicode category
                
                    // Backreferences
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case 'k':
                    throw new NotImplementedException();

                case 'S':
                case 's':
                case 'W':
                case 'w':
                case 'D':
                case 'd':
                default:
                    lexer.EndState();
                    return new EscapeToken(lexer.CurrentChar);
            }
        }
    }
}