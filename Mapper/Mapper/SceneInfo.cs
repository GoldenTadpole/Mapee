using System.IO;
using WorldEditor;

namespace Mapper
{
    public struct SceneInfo
    {
        public Level Level { get; set; }
        public Dimension Dimension { get; set; }
        public IRegionStore RegionStore { get; set; }

        public SceneInfo(Level level, Dimension dimension)
        {
            Level = level;
            Dimension = dimension;
            RegionStore = RegionStoreFactory.CreateRegionStore(Level.Version.Version);

            string regionDirectory = $"{Level.Directory}\\{Dimension.GetRegionFolder(Level.Version.Version)}";
            RegionStore.Itemize(regionDirectory);
        }
    }
}
