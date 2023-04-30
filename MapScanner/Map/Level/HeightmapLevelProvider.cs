using WorldEditor;

namespace MapScanner
{
    public class HeightmapLevelProvider : ILevelProvider
    {
        public HeightmapCollection.Heightmap Heightmap { get; set; }
        private short[]? _values;

        public HeightmapLevelProvider(HeightmapCollection.Heightmap heightmap)
        {
            Heightmap = heightmap;
        }

        public short Provide(int x, int z)
        {
            if (_values is null)
            {
                if (Heightmap.Locker is null)
                {
                    _values = new short[256];
                }
                else 
                {
                    _values = new short[Heightmap.Locker.UnlockedArrayLength];
                    Heightmap.Unlock(_values);
                }
            }

            return (short)(_values[x + z * 16] - 1);
        }
    }
}
