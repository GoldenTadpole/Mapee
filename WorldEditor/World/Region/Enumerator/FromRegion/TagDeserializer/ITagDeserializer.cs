using NbtEditor;

namespace WorldEditor
{
    public interface ITagDeserializer
    {
        CompoundTag? Deserialize(ArraySlice<byte> decompressed, int iterator);
    }
}
