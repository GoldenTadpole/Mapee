using AssetSystem.Block;
using Mapper.Gui.Logic;
using Mapper.Gui.Model;
using MapScanner;
using System.Collections.Generic;

namespace Mapper.Gui.Controller
{
    public class ScanProfile : IScanProfile
    {
        public HeightmapSettings HeightmapProfile { get; set; }
        public ITokenizedBlockAsset<BlockGrouping> Asset { get; set; }

        public IList<string>? AllowedNbtHeightmaps { get; }
        public ITokenizedBlockAsset<BlockGrouping> DefaultAsset { get; }

        public ScanProfile(ProgramDomain domain, WorldDomain world) 
        {
            HeightmapProfile = world.CurrentDimension.HeightmapSettings;
            Asset = world.Style.AssetPack.BlockGroupingAsset;

            AllowedNbtHeightmaps = domain.ChunkMapperPack.NbtHeightmaps;
            DefaultAsset = domain.CurrentStyle.AssetPack.BlockGroupingAsset;
        }
    }
}
