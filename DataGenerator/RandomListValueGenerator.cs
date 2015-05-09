using System;
using System.Linq;
using DataGenerator.Core;

namespace DataGenerator
{
    /// <summary>
    /// Infinite value generator based on the values list.
    /// Probability of the value either normal or as specified in the weights array.
    /// </summary>
    public class RandomListValueGenerator<T> : IValueGenerator<T>
    {
        private readonly T[] list_;
        private readonly decimal weightsSum_;
        private readonly Tuple<decimal, int>[] sortedWeightsList_;
        private readonly Random random_;

        public RandomListValueGenerator(int seed, T[] list, decimal[] weightsList = null)
        {
            list_ = list;

            if (weightsList != null)
            {
                weightsSum_ = weightsList.Sum();
                sortedWeightsList_ = weightsList.Select((v, idx) => Tuple.Create(v, idx)).OrderBy(_ => _.Item1).ToArray();
            }
            random_ = new Random(seed);
        }

        public Option<T> Next()
        {
            var r = random_.NextDouble();
            if (sortedWeightsList_ == null)
            {
                int idx = (int)(r*list_.Length);
                return Option.Any(list_[idx]);
            }
            else
            {
                var weight = (decimal)r*weightsSum_;
                var sum = 0m;
                for (int i = 0; i < sortedWeightsList_.Length; i++)
                {
                    var item = sortedWeightsList_[i];
                    sum += item.Item1;

                    if (sum >= weight)
                        return Option.Any(list_[item.Item2]);
                }

                throw new InvalidOperationException();
            }
        }
    }
}