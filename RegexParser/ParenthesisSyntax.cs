namespace RegexParser
{
    public class ParenthesisSyntax : Syntax
    {
        public ParenthesisSyntax(ParenthesisRightToken token) :
            base(token)
        {
        }

        public override SyntaxType SyntaxType
        {
            get { return SyntaxType.Sequence; }
        }

        public override void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}