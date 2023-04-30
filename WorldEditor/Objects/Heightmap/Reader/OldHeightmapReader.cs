using NbtEditor;

namespace WorldEditor
{
    public class OldHeightmapReader : IObjectReader<ObjectReadParamter, OldHeightmap?>
    {
        public OldHeightmap? Read(ObjectReadParamter input)
        {
            if (!input.Level.TryGetChild(out Tag data, "Level", "HeightMap")) return null;

            return new OldHeightmap(ConvertValues(data))
            {
                DataTag = input.KeepDataTag ? data : null,
            };
        }

        protected virtual short[] ConvertValues(Tag arrayTag)
        {
            return Parser.ParseValues(arrayTag);
        }
    }
}
