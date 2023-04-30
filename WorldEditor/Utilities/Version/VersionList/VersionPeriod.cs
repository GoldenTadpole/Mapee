namespace WorldEditor
{
    public readonly struct VersionPeriod<TValue>
    {
        public Version BeginningVersion { get; }
        public Version EndVersion { get; }
        public TValue Value { get; }

        public VersionPeriod(Version beginningVersion, Version endVersion, TValue value)
        {
            BeginningVersion = beginningVersion;
            EndVersion = endVersion;
            Value = value;
        }
    }
}
