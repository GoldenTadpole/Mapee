using NbtEditor;

namespace WorldEditor
{
    public class ChunkVersionFinder : IChunkVersionFinder
    {
        public virtual Version FindVersion(ChunkParamater chunkParamater)
        {
            switch (chunkParamater.StorageFormat)
            {
                case StorageFormat.McRegion:
                    return Version.Post_Beta_1_3;
                case StorageFormat.Alpha:
                    return Version.Pre_Beta_1_2;
            }

            if (chunkParamater.Level.TryGetValue("DataVersion", out Tag? dataversionTag))
            {
                int dataversion = dataversionTag;
                if (Enum.IsDefined(typeof(Version), dataversion)) return (Version)dataversion;
                else
                {
                    if (dataversion > (int)Version.Newest.Prev()) return Version.Newest;
                }
            }

            return Version.Post_1_1;
        }
    }
}
