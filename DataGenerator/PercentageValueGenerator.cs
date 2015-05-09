using System;
using DataGenerator.Core;

namespace DataGenerator
{
    /// <summary>
    /// Generate random percent values. Sum of the all generated values will be 100%.
    /// </summary>
    public class PercentageValueGenerator : IValueGenerator<decimal>
    {
        private readonly int digits_;
        private readonly int count_;
        private readonly Random random_;
        private int generatedCount_;
        private readonly decimal[] prcBuf_;

        public PercentageValueGenerator(int seed, int count)
        {
            count_ = count;
            random_ = new Random(seed);
            prcBuf_ = Generate(random_, count);
        }

        public Option<decimal> Next()
        {
            generatedCount_++;
            if (generatedCount_ > count_)
                return Option.None<decimal>();
            
            return Option.Any(prcBuf_[generatedCount_ - 1]);
        }

        private static decimal[] Generate(Random random, int count)
        {
            decimal[] prcBuf = new decimal[count];
            var l = prcBuf.Length;
            decimal sum = 0;
            decimal[] buffer = new decimal[l];
            for (int i = 0; i < l; i++)
            {
                var nextDouble = (decimal)random.NextDouble();
                buffer[i] = nextDouble;
                sum += nextDouble;
            }

            decimal sum2 = 0;
            for (int i = 0; i < l - 1; i++)
            {
                var prc = Math.Round(buffer[i] / sum * 100, Settings.PercentDigits);
                sum2 += prc;
                prcBuf[i] = prc;
            }

            var last = 100.0m - sum2;
            prcBuf[l - 1] = last;
            return prcBuf;
        }
    }
}