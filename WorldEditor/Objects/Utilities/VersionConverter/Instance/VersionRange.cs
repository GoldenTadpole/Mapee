namespace WorldEditor
{
    public readonly struct VersionRange
    {
        public Version Start { get; init; }
        public Version End { get; init; }

        public VersionRange(Version start, Version end)
        {
            Start = start;
            End = end;
        }
        public VersionRange(Version version)
        {
            Start = version;
            End = version;
        }

        public bool IsInRange(Version version)
        {
            return version >= Start && version <= End;
        }
    }
}
