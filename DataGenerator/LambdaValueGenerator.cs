using System;
using DataGenerator.Core;

namespace DataGenerator
{
    /// <summary>
    /// The most generic lambda value generated.
    /// Will be used until lambda returns valid value.
    /// </summary>
    public class LambdaValueGenerator<T> : IValueGenerator<T>
    {
        private readonly Func<Option<T>> func_;

        public LambdaValueGenerator(Func<Option<T>> func)
        {
            func_ = func;
        }

        public Option<T> Next()
        {
            return func_();
        }
    }
}