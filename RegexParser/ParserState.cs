using System.Collections.Generic;

namespace RegexParser
{
    class ParserState
    {
        private readonly Stack<ISyntax> syntaxes_ = new Stack<ISyntax>();

        private bool alternation_;

        public void Handle(ParserStateIterator iterator)
        {
            IToken token = iterator.Current;
            switch (token.TokenType)
            {
                case TokenType.Literal:
                    LiteralToken literal = (LiteralToken)token;
                    ISyntax literalSyntax = new LiteralSyntax(literal);
                    AddSyntax(literalSyntax);
                    FlushAlternation();
                    break;

                case TokenType.Repetition:
                    RepetitionToken repetition = (RepetitionToken)token;
                    ISyntax repetitionSyntax = new RepetitionSyntax(repetition);
                    AddSyntax(repetitionSyntax);
                    FlushAlternation();
                    break;

                case TokenType.ParenthesisLeft:
                    iterator.ToState(new ParserState());
                    break;

                case TokenType.ParenthesisRight:
                    ParenthesisRightToken paranthesis = (ParenthesisRightToken)token;
                    ISyntax parenthesisSyntax = new ParenthesisSyntax(paranthesis);
                    AddSyntax(parenthesisSyntax);
                    iterator.EndState().FlushAlternation();
                    break;

                case TokenType.Alternation:
                    alternation_ = true;
                    break;

                case TokenType.Range: 
                    RangeToken range = (RangeToken)token;
                    ISyntax rangeSyntax = new RangeSyntax(range);
                    AddSyntax(new LiteralSyntax(new LiteralToken(range.EndChar)));
                    AddSyntax(rangeSyntax);
                    FlushAlternation();
                    break;

                case TokenType.Escape:
                    EscapeToken escape = (EscapeToken)token;
                    ISyntax escapeSyntax = new EscapeSyntax(escape);
                    AddSyntax(new LiteralSyntax(new LiteralToken(escape.Character)));
                    AddSyntax(escapeSyntax);
                    FlushAlternation();
                    break;

                case TokenType.BracketLeft:
                    iterator.ToState(new ParserState());
                    break;

                case TokenType.BracketRight:
                    BracketRightToken set = (BracketRightToken)token;
                    ISyntax bracketSyntax = new BracketSyntax(set);
                    AddSyntax(bracketSyntax);
                    iterator.EndState().FlushAlternation();
                    break;
                
                case TokenType.Any:
                    var any = (AnyToken)token;
                    ISyntax anySyntax = new AnySyntax(any);
                    AddSyntax(anySyntax);
                    break;
            }
        }

        private void FlushAlternation()
        {
            if (alternation_)
            {
                alternation_ = false;
                AddSyntax(new AlternationSyntax(new AlternationToken()));
            }
        }

        public void AddSyntax(ISyntax syntax)
        {
            syntaxes_.Push(syntax);
        }

        public ISyntax Process(HashSet<ISyntax> processedSyntaxes)
        {
            var syntax = syntaxes_.Pop();
            if (processedSyntaxes.Contains(syntax))
                return syntax;

            switch (syntax.SyntaxType)
            {
                case SyntaxType.Unary:
                    var u1 = Process(processedSyntaxes);
                    syntax.Children.Add(u1);
                    break;

                case SyntaxType.Binary:
                    syntax.Children.Insert(0, Process(processedSyntaxes));
                    syntax.Children.Insert(0, Process(processedSyntaxes));
                    break;

                case SyntaxType.Sequence:
                    while (syntaxes_.Count > 0)
                        syntax.Children.Insert(0, Process(processedSyntaxes));
                    break;
            }
            processedSyntaxes.Add(syntax);
            return syntax;
        }
    }
}