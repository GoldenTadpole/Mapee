using NbtEditor;

namespace WorldEditor
{
    public class BlockStateChunkReader : IObjectReader<ObjectReadParamter, BlockStateChunk?>
    {
        public VersionList<string[]> SectionVersions { get; set; }
        public VersionList<BlockStatePath> BlockStatePathVersions { get; set; }
        public IObjectReader<ListTag, Block[]> PaletteReader { get; set; }

        public bool RemoveEmptyAirSections { get; set; }

        public BlockStateChunkReader()
        {
            SectionVersions = ChunkUtilities.GetSectionVersions();
            BlockStatePathVersions = ChunkUtilities.GetBlockStateVersions();
            PaletteReader = new PaletteReader();
            RemoveEmptyAirSections = true;
        }

        public BlockStateChunk? Read(ObjectReadParamter input)
        {
            if (!SectionVersions.TryRetrieveValue(input.Version, out string[]? path)) return null;
            if (!BlockStatePathVersions.TryRetrieveValue(input.Version, out BlockStatePath blockStatePath)) return null;
            if (!input.Level.TryGetChild(out Tag sectionListTag, path)) return null;

            BlockStateChunk output = new()
            {
                DataTag = input.KeepDataTag ? sectionListTag : null
            };

            IBlockStateReader reader = ChunkUtilities.GetBlockStateReader(input.Version);
            IBlockStateWriter writer = ChunkUtilities.GetBlockStateWriter(input.Version);

            if (sectionListTag is not ListTag sectionList) return null;
            foreach (var sectionTag in sectionList)
            {
                if (sectionTag is not CompoundTag sectionCompound) continue;
                PaletteSection<Block>? section = ReadSection(input, sectionCompound, blockStatePath);
                if (section is not null)
                {
                    output.Sections.Add(section);

                    section.Locker = new BlockStateLocker(section)
                    {
                        Reader = reader,
                        Writer = writer
                    };
                }
            }

            if (output.Sections.Count < 1)
            {
                input.CancelChunk = true;
                return null;
            }

            return output;
        }

        protected virtual PaletteSection<Block>? ReadSection(ObjectReadParamter parameter, CompoundTag section, BlockStatePath path)
        {
            if (!section.TryGetChild(out Tag paletteTag, path.Palette)) return null;
            if (paletteTag is not ListTag paletteList) return null;

            Block[] palette = PaletteReader.Read(paletteList);

            if (RemoveEmptyAirSections && palette.Length == 1 && palette[0].Name == "minecraft:air") return null;

            long[] blockStates;
            if (section.TryGetChild(out Tag blockStateTag, path.Values))
            {
                blockStates = blockStateTag;
            }
            else
            {
                blockStates = Array.Empty<long>();
            }

            return new PaletteSection<Block>(palette, blockStates)
            {
                Y = section["Y"] ?? 0,
                DataTag = parameter.KeepDataTag ? section : null
            };
        }
    }
}
