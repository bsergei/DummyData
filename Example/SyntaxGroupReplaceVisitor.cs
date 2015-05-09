using System;
using System.Collections.Generic;
using System.Text;
using RegexParser;

namespace Example
{
    public class SyntaxGroupReplaceVisitor : ISyntaxVisitor
    {
        private readonly StringBuilder stringBuilder_ = new StringBuilder();
        private readonly IDictionary<string, string> substs_;

        public SyntaxGroupReplaceVisitor(IDictionary<string, string> substs)
        {
            substs_ = substs;
        }

        public override string ToString()
        {
            return stringBuilder_.ToString();
        }

        public void Visit(LiteralSyntax syntax)
        {
            stringBuilder_.Append((string)syntax.Token.Character.ToString());
        }

        public void Visit(AnySyntax syntax)
        {
            throw new NotImplementedException();
        }

        public void Visit(RepetitionSyntax syntax)
        {
            throw new NotImplementedException();
        }

        public void Visit(ParenthesisSyntax syntax)
        {
            var token = (ParenthesisRightToken)syntax.Token;
            string subst;
            if (substs_.TryGetValue(token.Name, out subst))
            {
                stringBuilder_.Append(subst);
            }
            else
            {
                foreach (ISyntax child in syntax.Children)
                {
                    child.Accept(this);
                }
            }
        }

        public void Visit(AlternationSyntax syntax)
        {
            throw new NotImplementedException();
        }

        public void Visit(RangeSyntax syntax)
        {
            throw new NotImplementedException();
        }

        public void Visit(BracketSyntax syntax)
        {
            throw new NotImplementedException();
        }

        public void Visit(EscapeSyntax syntax)
        {
            throw new NotImplementedException();
        }
    }
}