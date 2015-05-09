namespace RegexParser
{
    public class AlternationSyntax : Syntax
    {
        public AlternationSyntax(AlternationToken token)
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