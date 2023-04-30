using NbtEditor;

namespace WorldEditor
{
    public class AlphaBlockStateReader : IObjectReader<ObjectReadParamter, AlphaBlockState?>
    {
        public AlphaBlockState? Read(ObjectReadParamter input)
        {
            if (!input.Level.TryGetChild(out Tag blockStateTag, "Level", "Blocks") &
                !input.Level.TryGetChild(out Tag dataTag, "Level", "Data")) return null;

            return new AlphaBlockState((byte[])(Array)(sbyte[])blockStateTag, (byte[])(Array)(sbyte[])dataTag)
            {
                BlockStateDataTag = input.KeepDataTag ? blockStateTag : null,
                DataTag = input.KeepDataTag ? dataTag : null
            };
        }
    }
}
