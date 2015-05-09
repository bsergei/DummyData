using DataGenerator.Core;

namespace DataGenerator
{
    /// <summary>
    /// Sequence value generator based on the list of values.
    /// Will stop generator when all values used.
    /// </summary>
    public class ListValueGenerator<T> : IValueGenerator<T>
    {
        private readonly T[] list_;
        private readonly int count_;
        private int generatedCount_;
        private int index_;

        public ListValueGenerator(T[] list, int? count = null, int startingIndex = 0)
        {
            list_ = list;
            count_ = count ?? list.Length;
            index_ = startingIndex;
            NormalizeIndex();
        }

        public Option<T> Next()
        {
            generatedCount_++;
            if (generatedCount_ > count_)
                return Option.None<T>();

            var value = list_[index_];
            index_++;
            NormalizeIndex();
            return Option.Any(value);
        }

        private void NormalizeIndex()
        {
            if (index_ > list_.Length)
                index_ = 0;
        }
    }
}
