namespace RegexParser
{
    public interface ILexerState
    {
        IToken Handle(ILexer lexer);
    }
}