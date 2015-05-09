namespace RegexParser
{
    public class RangeSyntax : Syntax
    {
        public RangeSyntax(RangeToken token)
            : base(token) { }

        public override SyntaxType SyntaxType
        {
            get { return SyntaxType.Binary; }
        }

        public override void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}