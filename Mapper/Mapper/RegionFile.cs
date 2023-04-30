using WorldEditor;

namespace Mapper
{
    public readonly struct RegionFile
    {
        public Coords Coords { get; init; }
        public string FileName { get; init; }

        public RegionFile(Coords coords, string fileName)
        {
            Coords = coords;
            FileName = fileName;
        }

        public override int GetHashCode()
        {
            return Coords.GetHashCode();
        }
    }
}
