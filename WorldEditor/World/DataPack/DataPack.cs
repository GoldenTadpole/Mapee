namespace WorldEditor
{
    public class DataPack : IDataPack
    {
        public string Name { get; set; }
        public int Format { get; set; }
        public int Description { get; set; }

        public IList<CustomBiome> CustomBiomes { get; set; }
        IEnumerable<CustomBiome> IDataPack.CustomBiomes => CustomBiomes;

        public DataPack()
        {
            Name = string.Empty;
            CustomBiomes = new List<CustomBiome>();
        }
    }
}
