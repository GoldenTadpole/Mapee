using System.Text.Json.Nodes;

namespace AssetSystem.Block
{
    public struct BlockAssetPageReadArgs<TOutput> where TOutput : struct
    {
        public JsonArray PageNode { get; set; }
        public BlockAsset<TOutput> Output { get; set; }

        public BlockAssetPageReadArgs(JsonArray pageNode, BlockAsset<TOutput> output)
        {
            PageNode = pageNode;
            Output = output;
        }
    }
}
