namespace RegexParser
{
    public class LiteralSyntax : Syntax
    {
        public LiteralSyntax(LiteralToken token) :
            base(token)
        {
        }

        public new LiteralToken Token
        {
            get
            {
                return (LiteralToken)base.Token;
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