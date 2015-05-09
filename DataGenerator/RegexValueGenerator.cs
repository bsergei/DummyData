using System;
using System.Text;
using DataGenerator.Core;
using RegexParser;

namespace DataGenerator
{
    /// <summary>
    /// Generates any number of random values, restricted by Refex template.
    /// Supports '/w', '/d', '.' or custom char ranges with any cardinality 
    /// specified as well as  literals and alternation (see nested 
    /// SyntaxVisitor class for more for all options).
    /// E.g. "(MyRandomValue\d{2})|(MyAnoterRandomValue_\w+)" will produce 
    /// possible values "MyRandomValue52", "MyAnoterRandomValue_fgjd", etc.
    /// </summary>
    public class RegexValueGenerator : IValueGenerator<string>
    {
        private readonly Random random_;
        private readonly ISyntax root_;

        public RegexValueGenerator(int randomSeed, string regexPattern)
        {
            root_ = new RegexParser.RegexParser(regexPattern).Root;
            random_ = new Random(randomSeed);
        }

        public Option<string> Next()
        {
            return Option.Any(SyntaxVisitor.Visit(random_, root_));
        }

        private class SyntaxVisitor : ISyntaxVisitor
        {
            private readonly StringBuilder builder_ = new StringBuilder();
            private readonly Random random_;

            private SyntaxVisitor(Random random)
            {
                random_ = random;
            }

            private int GetRandomNumber(int min, int max)
            {
                return random_.Next(min, max);
            }

            public static string Visit(Random random, ISyntax syntax)
            {
                SyntaxVisitor syntaxVisitor = new SyntaxVisitor(random);
                syntax.Accept(syntaxVisitor);
                return syntaxVisitor.builder_.ToString();
            }

            public void Visit(LiteralSyntax syntax)
            {
                builder_.Append((string) syntax.Token.Character.ToString());
            }

            public void Visit(AnySyntax syntax)
            {
                char c = (char) GetRandomNumber(32, 127);
                LiteralSyntax literal = new LiteralSyntax(new LiteralToken(c));
                literal.Accept(this);
            }

            public void Visit(RepetitionSyntax syntax)
            {
                int maxOccurs = syntax.Token.MaxOccurs ?? 10;
                int repeatCount = GetRandomNumber(syntax.Token.MinOccurs, maxOccurs);
                for (int i = 0; i < repeatCount; i++)
                {
                    foreach (ISyntax child in syntax.Children)
                    {
                        child.Accept(this);
                    }
                }
            }

            public void Visit(AlternationSyntax syntax)
            {
                int index = GetRandomNumber(0, 2);
                syntax.Children[index].Accept(this);
            }

            public void Visit(ParenthesisSyntax syntax)
            {
                foreach (ISyntax child in syntax.Children)
                {
                    child.Accept(this);
                }
            }

            public void Visit(BracketSyntax syntax)
            {
                if (syntax.Token.IsNegate)
                    throw new NotSupportedException();

                int index = GetRandomNumber(0, syntax.Children.Count);
                syntax.Children[index].Accept(this);
            }

            public void Visit(RangeSyntax syntax)
            {
                int min = (int)((LiteralSyntax)syntax.Children[0]).Token.Character;
                int max = (int)((LiteralSyntax)syntax.Children[1]).Token.Character;
                int index = GetRandomNumber(min, max + 1);
                LiteralSyntax literal = new LiteralSyntax(new LiteralToken((char)index));
                literal.Accept(this);
            }

            private const string WChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHIJKMNOPQRSTUVWXYZ";

            public void Visit(EscapeSyntax syntax)
            {
                var literalSyntax = (LiteralSyntax)syntax.Children[0];
                switch (literalSyntax.Token.Character)
                {
                    case 'd':
                        {
                            int index = GetRandomNumber('0', '9' + 1);
                            LiteralSyntax literal = new LiteralSyntax(new LiteralToken((char)index));
                            literal.Accept(this);
                        }
                        break;

                    case 'w':
                        {
                            int index = GetRandomNumber(0, WChars.Length);
                            LiteralSyntax literal = new LiteralSyntax(new LiteralToken(WChars[index]));
                            literal.Accept(this);
                        }
                        break;

                    case 's':
                    case 'W':
                    case 'D':
                        throw new NotSupportedException();

                    default:
                        literalSyntax.Accept(this);
                        break;
                }
            }
        }
    }
}
