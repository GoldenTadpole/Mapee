using NbtEditor;

namespace WorldEditor
{
    public class AnvilBlockStateChunk : IObject
    {
        public Tag? DataTag { get; set; }
        public IList<Section> Sections { get; set; }

        public class Section : ISection, IObject
        {
            public sbyte Y { get; set; }
            public Tag? DataTag { get; set; }
            public Tag? BlockStateDataTag { get; set; }

            public byte[] BlockStates { get; set; }
            public byte[] BlockData { get; set; }

            public Section(byte[] blockStates, byte[] blockData) 
            {
                BlockStates = blockStates;
                BlockData = blockData;
            }
        }

        public AnvilBlockStateChunk()
        {
            Sections = new List<Section>();
        }
    }
}
