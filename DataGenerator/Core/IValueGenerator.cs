namespace DataGenerator.Core
{
    public interface IValueGenerator<T>
    {
        Option<T> Next();
    }
}