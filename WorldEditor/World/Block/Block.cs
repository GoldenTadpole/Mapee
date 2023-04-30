namespace WorldEditor
{
    public readonly struct Block
    {
        public string Name { get; init; }
        public Property[] Properties { get; init; }

        public static Block Empty => new(string.Empty, Array.Empty<Property>());

        public Block()
        {
            Name = string.Empty;
            Properties = Array.Empty<Property>();
        }
        public Block(string name)
        {
            Name = name;
            Properties = Array.Empty<Property>();
        }
        public Block(string name, Property[] properties)
        {
            Name = name;
            Properties = properties;
        }

        public bool IsEmpty() => string.IsNullOrEmpty(Name);

        public bool Equals(Block other)
        {
            if (Name != other.Name ||
                Properties.Length != other.Properties.Length) return false;

            for (int i = 0; i < Properties.Length; i++)
            {
                if (Properties[i].Name != other.Properties[i].Name ||
                    Properties[i].Value != other.Properties[i].Value) return false;
            }

            return true;
        }
    }
}
