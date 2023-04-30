namespace WorldEditor
{
    public interface IDataPack
    {
        string Name { get; }
        int Format { get; }
        int Description { get; }

        IEnumerable<CustomBiome> CustomBiomes { get; }
    }
}
