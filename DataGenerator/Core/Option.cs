namespace DataGenerator.Core
{
    public abstract class Option
    {
        protected Option(bool hasValue)
        {
            hasValue_ = hasValue;
        }

        private readonly bool hasValue_;
        public bool HasValue
        {
            get { return hasValue_; }
        }

        public static Option<T> Any<T>(T value)
        {
            return new Option<T>(true, value);
        }

        public static Option<T> None<T>()
        {
            return new Option<T>(false, default(T));
        }
    }

    public class Option<T> : Option
    {
        private readonly T value_;

        public Option(bool hasValue, T value) : base(hasValue)
        {
            value_ = value;
        }

        public T Value
        {
            get { return value_; }
        }
    }
}