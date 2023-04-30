namespace MapScanner
{
    public class SimpleLevelProvider : ILevelProvider
    {
        public short Y { get; set; }

        public SimpleLevelProvider(short y)
        {
            Y = y;
        }

        public short Provide(int x, int z)
        {
            return Y;
        }
    }
}
