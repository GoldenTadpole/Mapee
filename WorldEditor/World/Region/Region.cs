namespace WorldEditor
{
    public class Region
    {
        public int X { get; set; }
        public int Z { get; set; }

        public IList<IChunk> Chunks { get; set; }

        public Region()
        {
            X = 0;
            Z = 0;
            Chunks = new List<IChunk>(1024);
        }
        public Region(int x, int z)
        {
            X = x;
            Z = z;
            Chunks = new List<IChunk>(1024);
        }
        public Region(int x, int z, IChunk[] chunks)
        {
            X = x;
            Z = z;
            Chunks = new List<IChunk>(chunks);
        }
    }
}
