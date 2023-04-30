namespace WorldEditor
{
    public class HeightmapLockerInstanceConverter : IInstanceConverter<IObject?>
    {
        public VersionRange From { get; set; } = new VersionRange(Version.Snapshot_18w06a, Version.Snapshot_20w16a);
        public VersionRange To { get; set; } = new VersionRange(Version.Snapshot_20w17a, Version.Snapshot_21w05b);
        public short FloorY { get; set; } = 0;

        public IObject? Convert(IObject input, UsageIntent intent)
        {
            if (input is not HeightmapCollection output) return null;

            IBlockStateReader reader = ChunkUtilities.GetBlockStateReader(To.End);
            IBlockStateWriter writer = ChunkUtilities.GetBlockStateWriter(To.End);

            foreach (var heightmap in output.Heightmaps)
            {
                if (heightmap.Value.Locker is null) continue;

                heightmap.Value.Locker.Writer = writer;
                heightmap.Value.FloorY = FloorY;

                if (intent == UsageIntent.ReadWrite)
                {
                    short[] unlockedArray = new short[heightmap.Value.Locker.UnlockedArrayLength];
                    heightmap.Value.Unlock(unlockedArray);

                    heightmap.Value.Lock(unlockedArray);
                    heightmap.Value.Locker.Reader = reader;
                }
            }

            return output;
        }
    }
}
