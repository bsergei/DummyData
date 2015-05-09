namespace RegexParser
{
    public class RegexParser
    {
        private readonly string expression_;
        private ISyntax root_;

        public RegexParser(string expression)
        {
            expression_ = expression;
        }

        public ISyntax Root
        {
            get { return root_ ?? (root_ = new ParserStateIterator().Parse(expression_)); }
        }
    }
}