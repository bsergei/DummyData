namespace RegexParser
{
    public class EscapeToken : IToken
    {
        private readonly char character_;

        public EscapeToken(char character)
        {
            character_ = character;
        }

        public TokenType TokenType { get { return TokenType.Escape; } }

        public char Character
        {
            get { return character_; }
        }
    }
}