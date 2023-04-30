using WorldEditor;

namespace Mapper
{
    public struct DataPackAssetPackFactoryArgs
    {
        public IEnumerable<IDataPack> DataPacks { get; set; }
        public Colormap Colormap { get; set; }
    }
}
