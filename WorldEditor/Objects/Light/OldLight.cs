using NbtEditor;

namespace WorldEditor
{
    public class OldLight : IObject
    {
        public Tag? DataTag { get; set; }
        public byte[] Values { get; set; }

        public OldLight(byte[] values) 
        {
            Values = values;
        }
    }
}
