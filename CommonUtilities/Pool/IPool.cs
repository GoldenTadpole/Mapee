namespace CommonUtilities.Pool
{
    public interface IPool<out TObject>
    {
        TObject Provide();
    }
    public interface IPool<in TInput, out TObject>
    {
        TObject Provide(TInput input);
    }
}
