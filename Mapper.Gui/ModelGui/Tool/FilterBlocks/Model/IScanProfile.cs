using AssetSystem.Block;
using MapScanner;
using System.Collections.Generic;

namespace Mapper.Gui.Model
{
    public interface IScanProfile
    {
        HeightmapSettings HeightmapProfile { get; set; }
        ITokenizedBlockAsset<BlockGrouping> Asset { get; set; }

        IList<string>? AllowedNbtHeightmaps { get; }
        ITokenizedBlockAsset<BlockGrouping> DefaultAsset { get; }
    }
}
