namespace WorldEditor
{
    public interface IRegionStore
    {
        int Count { get; }

        IEnumerable<Coords> Itemize(string directory);
        bool Exists(Coords coords);
        bool GetData(Coords coords, out byte[]? buffer, out StorageFormat format);
    }
}
