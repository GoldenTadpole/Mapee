using Mapper.Gui.Model;

namespace Mapper.Gui.Logic
{
    public class Profile
    {
        public AssetPack AssetPack { get; set; }
        public HeightmapSettings HeightmapSettings { get; set; }
        public RenderSettings RenderSettings { get; set; }

        public Profile(AssetPack assetPack, HeightmapSettings heightmapProfile, RenderSettings renderSettings) 
        {
            AssetPack = assetPack;
            HeightmapSettings = heightmapProfile;
            RenderSettings = renderSettings;
        }
    }
}
