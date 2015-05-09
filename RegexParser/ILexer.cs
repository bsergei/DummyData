namespace RegexParser
{
    public interface ILexer
    {
        void ToState(ILexerState state);
        void EndState();
        char CurrentChar { get; }
    }
}