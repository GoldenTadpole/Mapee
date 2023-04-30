using AssetSystem;
using AssetSystem.Biome;
using WorldEditor;

namespace Mapper
{
    public class CustomBiomeAssetReader : IObjectReader<DataPackAssetPackFactoryArgs, IBiomeAsset<VecRgb>?>
    {
        public static string[] Grass { get; } = new string[] {
            "minecraft:grass_block",
            "minecraft:grass",
            "minecraft:tall_grass",
            "minecraft:fern",
            "minecraft:large_fern",
            "minecraft:potted_fern",
            "minecraft:sugar_cane"
        };
        public static string[] Folliage { get; } = new string[] {
            "minecraft:oak_leaves",
            "minecraft:jungle_leaves",
            "minecraft:acacia_leaves",
            "minecraft:dark_oak_leaves",
            "minecraft:mangrove_leaves",
            "minecraft:vine"
        };
        public static string[] Water { get; } = new string[] {
            "minecraft:water",
            "minecraft:water_cauldron",
            "minecraft:bubble_column"
        };

        public IObjectReader<BiomeColorReadArgs, VecRgb> BiomeColorReader { get; set; }

        public CustomBiomeAssetReader()
        {
            BiomeColorReader = new BiomeAssetColorReader();
        }

        public IBiomeAsset<VecRgb>? Read(DataPackAssetPackFactoryArgs args)
        {
            BiomeAsset<VecRgb> output = new();

            foreach (IDataPack dataPack in args.DataPacks)
            {
                ReadDataPack(dataPack, args, output);
            }

            if (output.BiomeBlocks.Count < 1) return null;

            return output;
        }
        private void ReadDataPack(IDataPack dataPack, DataPackAssetPackFactoryArgs args, BiomeAsset<VecRgb> output)
        {
            foreach (CustomBiome biome in dataPack.CustomBiomes)
            {
                AddBiome(biome.GrassColor, args.Colormap.Grass, Grass);
                AddBiome(biome.FolliageColor, args.Colormap.Foliage, Folliage);
                AddBiome(biome.WaterColor, null, Water);

                void AddBiome(IBiomeColor? color, IReadOnlyBitmap? colormap, string[] blocks)
                {
                    if (color is null) return;
                    foreach (string block in blocks)
                    {
                        VecRgb rgb = BiomeColorReader.Read(new BiomeColorReadArgs(color, colormap));
                        if (rgb.IsEmpty()) continue;

                        output.Add(new BiomeBlock(new Block(block), biome.Namespace), new PropertyMatcher<VecRgb>(rgb, null));
                    }
                }
            }
        }
    }
}
