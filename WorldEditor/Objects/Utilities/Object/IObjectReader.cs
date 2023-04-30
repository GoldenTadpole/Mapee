namespace WorldEditor
{
    public interface IObjectReader<in TInput>
    {
        void Read(TInput input);
    }
    public interface IObjectReader<in TInput, out TOutput>
    {
        TOutput Read(TInput input);
    }
}
