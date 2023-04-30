using NbtEditor;

namespace WorldEditor
{
    public class LightReader : IObjectReader<ObjectReadParamter, LightChunk?>
    {
        public VersionList<string[]> SectionVersions { get; set; }
        public string TagName { get; set; }

        public LightReader(string tagName)
        {
            SectionVersions = ChunkUtilities.GetSectionVersions();
            TagName = tagName;
        }

        public LightChunk? Read(ObjectReadParamter input)
        {
            if (!SectionVersions.TryRetrieveValue(input.Version, out string[]? path)) return null;
            if (!input.Level.TryGetChild(out Tag sectionListTag, path)) return null;
            if (sectionListTag is not ListTag sections) return null;

            LightChunk output = new()
            {
                DataTag = input.KeepDataTag ? sectionListTag : null
            };

            for (int i = 0; i < sections.Count; i++)
            {
                if (sections[i] is not CompoundTag sectionCompound) continue;

                LightChunk.Section? section = ReadSection(input, sectionCompound);
                if (section is not null) output.Sections.Add(section);
            }

            return output;
        }
        protected virtual LightChunk.Section? ReadSection(ObjectReadParamter input, CompoundTag sectionTag)
        {
            if (!sectionTag.TryGetValue(TagName, out Tag? blockLightTag)) return null;

            return new LightChunk.Section((byte[])(Array)(sbyte[])blockLightTag)
            {
                DataTag = input.KeepDataTag ? blockLightTag : null,
                Y = sectionTag["Y"] ?? 0
            };
        }
    }
}
