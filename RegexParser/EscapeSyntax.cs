namespace RegexParser
{
    public class EscapeSyntax : Syntax
    {
        public EscapeSyntax(EscapeToken escape) : base(escape)
        {
        }

        public new EscapeToken Token
        {
            get
            {
                return (EscapeToken)base.Token;
            }
        }

        public override SyntaxType SyntaxType
        {
            get { return SyntaxType.Unary; }
        }

        public override void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}