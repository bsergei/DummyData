using System.Collections.Generic;

namespace RegexParser
{
    public interface ISyntax
    {
        IList<ISyntax> Children { get; }
        
        IToken Token { get; }
        
        SyntaxType SyntaxType { get; }

        void Accept(ISyntaxVisitor visitor);
    }
}