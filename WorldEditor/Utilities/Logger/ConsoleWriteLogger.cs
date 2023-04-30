namespace WorldEditor
{
    public class ConsoleWriteLogger<TMessage> : ILogger<TMessage>
    {
        public void Log(TMessage message)
        {
            Console.WriteLine(message);
        }
    }
}
