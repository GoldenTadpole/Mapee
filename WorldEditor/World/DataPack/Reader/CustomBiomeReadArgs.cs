namespace WorldEditor
{
    public readonly struct CustomBiomeReadArgs
    {
        public string Namespace { get; init; }
        public string? Dimension { get; init; }

        public string FileName { get; init; }
        public byte[] FileContents { get; init; }

        public CustomBiomeReadArgs(string nameSpace, string fileName, byte[] fileContents)
        {
            Namespace = nameSpace;
            FileName = fileName;
            FileContents = fileContents;
        }
    }
}
