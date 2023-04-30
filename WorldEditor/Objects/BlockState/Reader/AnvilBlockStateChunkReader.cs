using NbtEditor;

namespace WorldEditor
{
    public class AnvilBlockStateChunkReader : IObjectReader<ObjectReadParamter, AnvilBlockStateChunk?>
    {
        public AnvilBlockStateChunk? Read(ObjectReadParamter input)
        {
            if (!input.Level.TryGetChild(out Tag sectionListTag, "Level", "Sections")) return null;

            AnvilBlockStateChunk output = new()
            {
                DataTag = input.KeepDataTag ? sectionListTag : null
            };

            if (sectionListTag is not ListTag sectionList) return null;
            foreach (var sectionTag in sectionList)
            {
                if (sectionTag is not CompoundTag sectionCompound) continue;

                AnvilBlockStateChunk.Section? section = ReadSection(input, sectionCompound);
                if (section is not null) output.Sections.Add(section);
            }

            return output;
        }
        protected virtual AnvilBlockStateChunk.Section? ReadSection(ObjectReadParamter parameter, CompoundTag section)
        {
            if (!section.TryGetValue("Blocks", out Tag? blockStateTag) &
                !section.TryGetValue("Data", out Tag? dataTag)) return null;
            if (blockStateTag is null || dataTag is null) return null;

            return new AnvilBlockStateChunk.Section((byte[])(Array)(sbyte[])blockStateTag, (byte[])(Array)(sbyte[])dataTag)
            {
                BlockStateDataTag = parameter.KeepDataTag ? blockStateTag : null,
                DataTag = parameter.KeepDataTag ? dataTag : null,
                Y = section["Y"] ?? 0,
            };
        }
    }
}
