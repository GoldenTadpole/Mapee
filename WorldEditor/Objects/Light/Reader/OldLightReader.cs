using NbtEditor;

namespace WorldEditor
{
    public class OldLightReader : IObjectReader<ObjectReadParamter, OldLight?>
    {
        public string TagName { get; set; }

        public OldLightReader(string tagName)
        {
            TagName = tagName;
        }

        public OldLight? Read(ObjectReadParamter input)
        {
            if (!input.Level.TryGetChild(out Tag blockLightTag, "Level", TagName)) return null;

            return new OldLight((byte[])(Array)(sbyte[])blockLightTag)
            {
                DataTag = input.KeepDataTag ? blockLightTag : null
            };
        }
    }
}
