using System.Diagnostics.CodeAnalysis;

namespace WorldEditor
{
    public readonly struct Dimension
    {
        public string Namespace { get; init; }
        public string Name { get; init; }

        public static bool operator ==(Dimension left, Dimension right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Dimension left, Dimension right)
        {
            return !(left == right);
        }

        public static readonly Dimension Overworld = new("minecraft:overworld", "Overworld");
        public static readonly Dimension Nether = new("minecraft:nether", "Nether");
        public static readonly Dimension TheEnd = new("minecraft:the_end", "The End");

        public Dimension(string @namespace, string name) 
        {
            Namespace = @namespace;
            Name = name;
        }
        public Dimension(string @namespace)
        {
            Namespace = @namespace;
            Name = @namespace;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not Dimension dimension) return false;
            return Namespace == dimension.Namespace;
        }
        public override int GetHashCode()
        {
            return Namespace.GetHashCode();
        }
    }
}
