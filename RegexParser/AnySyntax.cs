namespace RegexParser
{
    public class AnySyntax : Syntax
    {
        public AnySyntax(AnyToken escape)
            : base(escape)
        {
        }

        public new AnyToken Token
        {
            get
            {
                return (AnyToken)base.Token;
            }
        }

        public override SyntaxType SyntaxType
        {
            get { return SyntaxType.Operand; }
        }

        public override void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}