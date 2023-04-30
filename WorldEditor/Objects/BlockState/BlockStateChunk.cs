using CommonUtilities.Collections.Simple;
using NbtEditor;

namespace WorldEditor
{
    public class BlockStateChunk : IObject
    {
        public Tag? DataTag { get; set; }
        public IList<PaletteSection<Block>> Sections { get; set; }

        public BlockStateChunk(int count = 25)
        {
            Sections = new SimpleList<PaletteSection<Block>>(count);
        }
    }
}
