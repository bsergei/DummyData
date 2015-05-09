namespace RegexParser
{
    public class RepetitionSyntax : Syntax
    {
        public RepetitionSyntax(RepetitionToken token) :
            base(token){}

        public new RepetitionToken Token
        {
            get
            {
                return (RepetitionToken)base.Token;
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