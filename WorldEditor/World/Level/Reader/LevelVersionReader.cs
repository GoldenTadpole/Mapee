using NbtEditor;

namespace WorldEditor
{
    public class LevelVersionReader : IObjectReader<LevelArgs, LevelVersion>
    {
        public LevelVersion Read(LevelArgs input)
        {
            LevelVersion version = new();

            if (input.Data.TryGetChild("DataVersion", out Tag dataVersionTag))
            {
                version.Version = (Version)(int)dataVersionTag;
            }
            else 
            {
                version.Version = Version.Unknown;
            }

            if (input.Data.TryGetChild(out Tag versionNameTag, "Version", "Name"))
            {
                version.VersionName = versionNameTag;
            }
            else 
            {
                version.VersionName = "Unknown";
            }

            return version;
        }
    }
}
