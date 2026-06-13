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

        public string? GetRegionFolder(Version version)
        {
            if (version < Version.Post_Beta_1_3)
            {
                if (this == Nether) return "DIM-1";
                if (this == TheEnd) return "DIM1";
                return "";
            }

            string[] split = Namespace.Split(':');
            if (split.Length < 2) return null;

            string @namespace = split[0];
            string name = split[1];

            if (version < Version.Snapshot_26_1_6 && Namespace.StartsWith("minecraft"))
            {
                if (this == Overworld) return "region";
                if (this == Nether) return "DIM-1\\region";
                if (this == TheEnd) return "DIM1\\region";
                return "region";
            }

            return $"dimensions\\{@namespace}\\{name}\\region";
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
