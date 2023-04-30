using NbtEditor;
using System.Numerics;

namespace WorldEditor
{
    public class WorldGenReader : IObjectReader<LevelArgs, WorldGen>
    {
        private static readonly string[][] SEED_PATHS = new string[][] 
        {
            new[] { "WorldGenSettings", "seed" },
            new[] { "RandomSeed" }
        };

        public WorldGen Read(LevelArgs input)
        {
            return new WorldGen() 
            {
                WorldSpawn = ReadSpawn(input.Data),
                Seed = ReadSeed(input.Data)
            };
        }

        private static Vector3 ReadSpawn(CompoundTag data) 
        {
            int x = 0, y = 0, z = 0;
            if (data.TryGetChild("SpawnX", out Tag xTag) && xTag is ValueTag<int> xValueTag)
            {
                x = xValueTag;
            }
            if (data.TryGetChild("SpawnY", out Tag yTag) && yTag is ValueTag<int> yValueTag)
            {
                y = yValueTag;
            }
            if (data.TryGetChild("SpawnZ", out Tag zTag) && zTag is ValueTag<int> zValueTag)
            {
                z = zValueTag;
            }

            return new Vector3(x, y, z);
        }
        private static long ReadSeed(CompoundTag data) 
        {
            for (int i = 0; i < SEED_PATHS.Length; i++) 
            {
                if (data.TryGetChild(out Tag tag, SEED_PATHS[i]) && tag.Id == TagId.Int64) 
                {
                    return tag;
                }
            }

            return 0;
        }
    }
}
