namespace WorldEditor
{
    public class HeightmapExtenderInstanceConverter : IInstanceConverter<IObject?>
    {
        public VersionRange From { get; set; } = new VersionRange(Version.Snapshot_20w17a, Version.Snapshot_21w05b);
        public VersionRange To { get; set; } = new VersionRange(Version.Snapshot_21w06a, Version.Newest);
        public short FloorY { get; set; } = -64;

        public IObject? Convert(IObject input, UsageIntent intent)
        {
            if (input is not HeightmapCollection output) return null;

            for (int i = 0; i < output.Heightmaps.Count; i++)
            {
                output.Heightmaps.ElementAt(i).Value.FloorY = FloorY;
            }

            return output;
        }
    }
}
