namespace RegexParser
{
    public class RepetitionToken : IToken
    {
        public RepetitionToken(int minOccurs, int? maxOccurs)
        {
            MinOccurs = minOccurs;
            MaxOccurs = maxOccurs;
        }

        public int MinOccurs { get; private set; }
        public int? MaxOccurs { get; private set; }
        public TokenType TokenType { get { return TokenType.Repetition; } }
    }
}