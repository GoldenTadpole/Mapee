using NbtEditor;

namespace WorldEditor
{
    public class OldBiomeReader : IObjectReader<ObjectReadParamter, OldBiome?>
    {
        public OldBiome? Read(ObjectReadParamter input)
        {
            if (!input.Level.TryGetChild(out Tag biomeTag, "Level", "Biomes")) return null;

            return new OldBiome(ConvertValues(biomeTag))
            {
                DataTag = input.KeepDataTag ? biomeTag : null
            };
        }

        protected virtual short[] ConvertValues(Tag arrayTag)
        {
            return Parser.ParseValues(arrayTag);
        }
    }
}
