namespace CommonUtilities.Pool
{
    public interface IResettablePool<TOutput> : IPool<TOutput>
    {
        void Reset();
    }
    public interface IResettablePool<TInput, TOutput> : IPool<TInput, TOutput>
    {
        void Reset();
    }
}
