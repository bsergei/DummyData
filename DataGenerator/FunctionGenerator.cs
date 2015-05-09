using System;
using DataGenerator.Core;

namespace DataGenerator
{
    /// <summary>
    /// Generates specified max number of values, calculated by function.
    /// Function gets two parameters: current index and max count. And returns decimal value.
    /// Generator will stop when all values generated.
    /// </summary>
    public class FunctionGenerator : IValueGenerator<double>
    {
        private readonly int count_;
        private readonly Func<int, int, double> func_;
        private int generatedCount_;

        public FunctionGenerator(int count, Func<int, int, double> func)
        {
            count_ = count;
            func_ = func;
        }

        public Option<double> Next()
        {
            generatedCount_++;
            if (generatedCount_ > count_)
                return Option.None<double>();

            var y = func_(generatedCount_ - 1, count_);
            return Option.Any(y);
        }
    }
}