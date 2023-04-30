using Mapper.Gui.Model;
using System.Collections.Generic;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class Style
    {
        public AssetPack AssetPack { get; set; }

        public IDictionary<string, HeightmapSettings> DimensionHeightmapStyles { get; set; }
        public HeightmapSettings DefaultHeightmapStyle { get; set; }

        public IDictionary<string, RenderSettings> DimensionRenderSettingStyles { get; set; }
        public RenderSettings DefaultRenderSettingStyle { get; set; }

        public ScanType ScanType { get; set; }
        public StyleMetadata Metadata { get; set; }

        public Style(AssetPack assetPack, StyleMetadata metadata)
        {
            AssetPack = assetPack;
            Metadata = metadata;

            DimensionHeightmapStyles = new Dictionary<string, HeightmapSettings>();
            DimensionRenderSettingStyles = new Dictionary<string, Model.RenderSettings>();
        }

        public bool IsDimensionAllowed(Dimension dimension) 
        {
            return Metadata.DimensionChecker.Run(propertyName => dimension.Namespace);
        }
        public Profile GetProfile(Dimension dimension) 
        {
            if (!DimensionHeightmapStyles.TryGetValue(dimension.Namespace, out HeightmapSettings heightmapSettings)) 
            {
                heightmapSettings = DefaultHeightmapStyle;
            }

            if (!DimensionRenderSettingStyles.TryGetValue(dimension.Namespace, out RenderSettings renderSettings)) 
            {
                renderSettings = DefaultRenderSettingStyle;
            }

            return new Profile(AssetPack, heightmapSettings, renderSettings);
        }
        public Style Clone() 
        {
            return new Style(AssetPack.Clone(), Metadata)
            {
                DimensionHeightmapStyles = DimensionHeightmapStyles,
                DefaultHeightmapStyle = DefaultHeightmapStyle,
                DimensionRenderSettingStyles = DimensionRenderSettingStyles,
                DefaultRenderSettingStyle = DefaultRenderSettingStyle,
                ScanType = ScanType
            };
        }
    }
}
