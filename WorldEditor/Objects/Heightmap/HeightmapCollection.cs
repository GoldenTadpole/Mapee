using NbtEditor;

namespace WorldEditor
{
    public class HeightmapCollection : IObject
    {
        public Tag? DataTag { get; set; }
        public IDictionary<string, Heightmap> Heightmaps { get; set; }

        public HeightmapCollection()
        {
            Heightmaps = new Dictionary<string, Heightmap>(4);
        }

        public class Heightmap : LockableObject
        {
            public long[] Indexes { get; set; }
            public short FloorY { get; set; } = -64;

            protected override long[] LockedArray
            {
                get => Indexes;
                set => Indexes = value;
            }

            public Heightmap(long[] indexes) : base()
            {
                Indexes = indexes;
                Locker = new HeightmapLocker()
                {
                    Reader = new SimpleBlockStateReader()
                };
            }

            public override void Lock(short[] unlockedArray)
            {
                short[] modified = new short[unlockedArray.Length];
                for (int i = 0; i < unlockedArray.Length; i++)
                {
                    modified[i] = (short)(unlockedArray[i] - FloorY);
                }

                base.Lock(modified);
            }
            public override void Unlock(short[] outputArray)
            {
                base.Unlock(outputArray);

                for (int i = 0; i < outputArray.Length; i++)
                {
                    outputArray[i] += FloorY;
                }
            }
        }
    }
}