using NbtEditor;

namespace WorldEditor
{
    public class OldBiome : IObject
    {
        public Tag? DataTag { get; set; }
        public short[] Values { get; set; }

        public OldBiome(short[] values) 
        {
            Values = values;
        }
    }
}