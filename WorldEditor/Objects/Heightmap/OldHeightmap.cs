using NbtEditor;

namespace WorldEditor
{
    public class OldHeightmap : IObject
    {
        public Tag? DataTag { get; set; }
        public short[] Values { get; set; }

        public OldHeightmap(short[] values) 
        {
            Values = values;
        }
    }
}