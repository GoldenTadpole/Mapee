using CommonUtilities.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Mapper.Gui.Logic
{
    public class ProgramDomain
    {
        public AssetPack DefaultAssetPack { get; set; }
        public IList<Style> Styles { get; set; }
        public Style CurrentStyle { get; set; }

        public TexturedAssetPack TexturePack { get; set; }
        public ChunkMapperPack ChunkMapperPack { get; set; }

        public WorldDomain? CurrentWorld { get; set; }

        public ProgramDomain()
        {
            AssetPackFactory assetPackFactory = new();
            DefaultAssetPack = assetPackFactory.Create(new DataReader("DefaultAsset"));

            Styles = LoadStyles();
            CurrentStyle = Styles[0];

            TexturePack = LoadTexturePack();
            ChunkMapperPack = new ChunkMapperPack(DefaultAssetPack);
        }

        private IList<Style> LoadStyles() 
        {
            List<Style> styles = new List<Style>();

            ISet<string> entries = new HashSet<string>();
            entries.UnionWith(Directory.GetFiles("Styles"));
            entries.UnionWith(Directory.GetDirectories("Styles"));

            StyleReader styleReader = new(DefaultAssetPack);
            foreach (string entry in entries) 
            {
                IDataReader reader = new DataReader(entry);
                Style? style = styleReader.Read(reader);
                if (style is null) continue;

                styles.Add(style);
            }

            return styles.OrderBy(x => x.Metadata?.OrderedIndex).ToList();
        }

        private static TexturedAssetPack LoadTexturePack() 
        {
            Colormap colormap = new Colormap(
                new ReadOnlyBitmap(new Uri("Textured\\colormap\\grass.png", UriKind.Relative)),
                new ReadOnlyBitmap(new Uri("Textured\\colormap\\foliage.png", UriKind.Relative)));

            return new TexturedAssetPack(new Mapper.TexturedAssetPack()
            {
                Colormap = colormap
            });

            //TexturedAssetPackFactory factory = new TexturedAssetPackFactory();
            //TexturedAssetArgs args = new TexturedAssetArgs("Textured", "");

            //Mapper.TexturedAssetPack pack = factory.Create(args);
            //TexturePack = new TexturedAssetPack()
            //{
            //    BlockColorAsset = pack.BlockColorAsset,
            //    BiomeColorAsset = pack.BiomeColorAsset,
            //    FilePath = args.TexturePackPath
            //};
        }
    }
}
