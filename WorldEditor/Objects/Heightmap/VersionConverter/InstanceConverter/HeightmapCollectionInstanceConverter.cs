namespace WorldEditor
{
    public class HeightmapCollectionInstanceConverter : IInstanceConverter<IObject?>
    {
        public VersionRange From => new(Version.Oldest, Version.Snapshot_18w05a);
        public VersionRange To => new(Version.Snapshot_18w06a, Version.Snapshot_20w16a);

        public IObject? Convert(IObject input, UsageIntent intent)
        {
            if (input is not OldHeightmap old) return null;

            HeightmapCollection.Heightmap newHeightmap = new HeightmapCollection.Heightmap(Array.Empty<long>())
            {
                Locker = new HeightmapLocker()
                {
                    Reader = ChunkUtilities.GetBlockStateReader(To.End),
                    Writer = ChunkUtilities.GetBlockStateWriter(To.End)
                },
                FloorY = 0
            };

            newHeightmap.Lock(old.Values);

            HeightmapCollection output = new HeightmapCollection();
            output.Heightmaps.Add("WORLD_SURFACE", newHeightmap);

            return output;
        }
    }
}
