using System.Text.Json.Nodes;

namespace AssetSystem.Biome.Reader
{
    public struct BiomeAssetPageReadArgs<TOutput> where TOutput : struct
    {
        public JsonObject PageNode { get; set; }
        public BiomeAsset<TOutput> Output { get; set; }

        public BiomeAssetPageReadArgs(JsonObject pageNode, BiomeAsset<TOutput> output) 
        {
            PageNode = pageNode;
            Output = output;
        }
    }
}
