namespace RegexParser
{
    public enum TokenType
    {
        Literal,
        Repetition,
        ParenthesisLeft,
        ParenthesisRight,
        Alternation,
        BracketLeft,
        BracketRight, 
        Range, 
        Any,
        Escape
    }
}