namespace WorldEditor
{
    public interface ILogger<TMessage>
    {
        void Log(TMessage message);
    }
}
