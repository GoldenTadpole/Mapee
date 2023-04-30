using CommonUtilities.Data;
using CommonUtilities.Factory;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class StyleReader : IObjectReader<IDataReader, Style?>
    {
        public IFactory<IDataReader, AssetPack> AssetPackFactory { get; set; }
        public IObjectReader<IDataReader, StyleMetadata?> MetadataReader { get; set; }
        public IObjectReader<StyleSettingsArgs> HeightmapProfileReader { get; set; }
        public IObjectReader<StyleSettingsArgs> RenderSettingsProfileReader { get; set; }
        public IObjectReader<StyleSettingsArgs> ScanTypeReader { get; set; }

        public StyleReader(AssetPack defaultAssetPack) 
        {
            AssetPackFactory = new AssetPackFactory() { Default = defaultAssetPack };
            MetadataReader = new StyleMetadataReader();
            HeightmapProfileReader = new HeightmapProfileReader();
            RenderSettingsProfileReader = new RenderSettingsReader();
            ScanTypeReader = new ScanTypeReader();
        }

        public Style? Read(IDataReader input)
        {
            IDataReader? childDataReader = input.CreateChild("Asset");
            if (childDataReader is null) return null;

            AssetPack pack = AssetPackFactory.Create(childDataReader);
            StyleMetadata? metadata = MetadataReader.Read(input);
            if (metadata is null) return null;

            Style output = new(pack, metadata);

            StyleSettingsArgs args = new(input, output);
            HeightmapProfileReader.Read(args);
            RenderSettingsProfileReader.Read(args);
            ScanTypeReader.Read(args);

            return output;
        }
    }
}
