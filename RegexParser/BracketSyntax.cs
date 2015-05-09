namespace RegexParser
{
    public class BracketSyntax : Syntax
    {
        public BracketSyntax(BracketRightToken token)
            : base(token) { }

        public override SyntaxType SyntaxType
        {
            get { return SyntaxType.Sequence; }
        }

        public override void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }

        public new BracketRightToken Token
        {
            get
            {
                return (BracketRightToken)base.Token;
            }
        }
    }
}