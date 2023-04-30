namespace Mapper
{
    public interface IQueue<T>
    {
        int Count { get; }

        T[] TakeFirst(int count);

        void RemoveFirst(int count);
        void ReplaceWith(ReadOnlyMemory<T> items);
    }
}
