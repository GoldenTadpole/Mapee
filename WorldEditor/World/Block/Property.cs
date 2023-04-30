namespace WorldEditor
{
    public readonly struct Property
    {
        public string Name { get; init; }
        public string Value { get; init; }

        public Property()
        {
            Name = string.Empty;
            Value = string.Empty;
        }
        public Property(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
