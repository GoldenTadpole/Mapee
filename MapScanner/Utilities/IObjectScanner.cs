namespace MapScanner
{
    public interface IObjectScanner<in TInput>
    {
        void Scan(TInput input);
    }
    public interface IObjectScanner<in TInput, out TOutput>
    {
        TOutput Scan(TInput input);
    }
}
