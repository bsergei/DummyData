using System.Collections.Generic;

namespace RegexParser
{
    public abstract class Syntax : ISyntax
    {
        protected Syntax(IToken token)
        {
            Children = new List<ISyntax>();
            Token = token;
        }

        public IList<ISyntax> Children { get; internal set; }

        public IToken Token { get; internal set; }

        public abstract SyntaxType SyntaxType { get; }

        public abstract void Accept(ISyntaxVisitor visitor);
    }
}