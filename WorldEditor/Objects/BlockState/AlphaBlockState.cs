using NbtEditor;

namespace WorldEditor
{
    public class AlphaBlockState : IObject
    {
        public Tag? DataTag { get; set; }
        public Tag? BlockStateDataTag { get; set; }

        public byte[] BlockStates { get; set; }
        public byte[] BlockData { get; set; }

        public AlphaBlockState(byte[] blockStates, byte[] blockData) 
        {
            BlockStates = blockStates;
            BlockData = blockData;
        }
    }
}
