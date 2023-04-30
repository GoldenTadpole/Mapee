using CommonUtilities.Collections.Simple;
using NbtEditor;

namespace WorldEditor
{
    public class BiomeChunk : IObject
    {
        public Tag? DataTag { get; set; }

        public IList<PaletteSection<string>> Sections { get; set; }

        public BiomeChunk(int count = 25)
        {
            Sections = new SimpleList<PaletteSection<string>>(count);
        }
    }
}
