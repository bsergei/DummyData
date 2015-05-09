namespace RegexParser
{
    public interface ISyntaxVisitor
    {
        void Visit(LiteralSyntax syntax);
        void Visit(RepetitionSyntax syntax);
        void Visit(ParenthesisSyntax syntax);
        void Visit(AlternationSyntax syntax);
        void Visit(RangeSyntax syntax); 
        void Visit(BracketSyntax syntax); 
        void Visit(EscapeSyntax syntax);
        void Visit(AnySyntax syntax);
    }
}