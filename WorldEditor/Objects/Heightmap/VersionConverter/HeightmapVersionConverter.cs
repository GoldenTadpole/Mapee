namespace WorldEditor
{
    public class HeightmapVersionConverter : VersionConverter
    {
        protected override void InitializeConverters()
        {
            Converters.Add(new HeightmapCollectionInstanceConverter());
            Converters.Add(new HeightmapLockerInstanceConverter());
            Converters.Add(new HeightmapExtenderInstanceConverter()
            {
                From = new VersionRange(Version.Snapshot_20w17a, Version.Snapshot_21w05b),
                To = new VersionRange(Version.Snapshot_21w06a, Version.Snapshot_21w14a),
                FloorY = 0
            });
            Converters.Add(new HeightmapExtenderInstanceConverter()
            {
                From = new VersionRange(Version.Snapshot_21w06a, Version.Snapshot_21w14a),
                To = new VersionRange(Version.Snapshot_21w15a, Version.Rel_1_17_1),
                FloorY = -64
            });
            Converters.Add(new HeightmapExtenderInstanceConverter()
            {
                From = new VersionRange(Version.Snapshot_21w15a, Version.Rel_1_17_1),
                To = new VersionRange(Version.ExperimentalSnapshot_1_18_ExperimentalSnapshot_1, Version.Newest),
                FloorY = 0
            });
        }
    }
}
