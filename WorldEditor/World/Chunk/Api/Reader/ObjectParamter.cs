using NbtEditor;

namespace WorldEditor
{
    public class ObjectReadParamter
    {
        public CompoundTag Level { get; set; }
        public Version Version { get; set; }
        public bool KeepDataTag { get; set; }
        public bool CancelChunk { get; set; }

        public ObjectReadParamter(CompoundTag level, Version version, bool keepDataTag)
        {
            Level = level;
            Version = version;
            KeepDataTag = keepDataTag;
            CancelChunk = false;
        }
    }
}
