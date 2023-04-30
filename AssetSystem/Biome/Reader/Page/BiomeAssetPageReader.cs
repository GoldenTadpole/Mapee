using AssetSystem.Biome.Reader;
using System.Text.Json.Nodes;

namespace AssetSystem.Biome
{
    public class BiomeAssetPageReader<TOutput> : IAssetReader<BiomeAssetPageReadArgs<TOutput>> where TOutput : struct 
    {
        public IAssetReader<BiomeReadArgs, TOutput?> PayloadReader { get; set; }

        public BiomeAssetPageReader(IAssetReader<BiomeReadArgs, TOutput?> payloadReader) 
        {
            PayloadReader = payloadReader;
        }

        public void Read(BiomeAssetPageReadArgs<TOutput> input)
        {
            JsonObject? header = input.PageNode["Header"]?.AsObject();
            if (header is null) return;

            string? payloadName = header["PayloadName"]?.GetValue<string>();
            if (payloadName is null) return;

            JsonObject? extra = header["Extra"]?.AsObject();

            JsonArray? blockArrayNode = input.PageNode["Blocks"]?.AsArray();
            if (blockArrayNode is null) return;

            JsonArray? biomeArrayNode = input.PageNode["Biomes"]?.AsArray();
            if (biomeArrayNode is null) return;

            IEnumerable<KeyValuePair<WorldEditor.Block, string?>> blocks = ReadBlocks(blockArrayNode);
            IEnumerable<KeyValuePair<string, TOutput>> biomes = ReadBiomes(biomeArrayNode, payloadName, extra);

            CombineBlocksWithBiomes(blocks, biomes, input.Output);
        }

        private static IEnumerable<KeyValuePair<WorldEditor.Block, string?>> ReadBlocks(JsonArray blockArrayNode)
        {
            List<KeyValuePair<WorldEditor.Block, string?>> output = new(blockArrayNode.Count);

            foreach (JsonNode? blockNode in blockArrayNode) 
            {
                if (blockNode is null) continue;

                KeyValuePair<WorldEditor.Block, string?>? block = ReadBlock(blockNode);
                if (block is null) continue;

                output.Add(block.Value);
            }

            return output;
        }
        private static KeyValuePair<WorldEditor.Block, string?>? ReadBlock(JsonNode blockNode)
        {
            string? blockName = GetBlockName(blockNode);
            if (blockName is null) return null;

            WorldEditor.Block block = new(blockName);
            string? propertyExpression = GetPropertyExpression(blockNode);

            return new KeyValuePair<WorldEditor.Block, string?>(block, propertyExpression);
        }
        private static string? GetBlockName(JsonNode blockNode)
        {
            string? blockName = ConvertJsonNodeToString(blockNode);
            if (blockName is not null) return blockName;

            if (blockNode is not JsonArray blockArray || blockArray.Count < 1) return null;
            return ConvertJsonNodeToString(blockArray[0]);
        }
        private static string? GetPropertyExpression(JsonNode blockNode)
        {
            if (blockNode is not JsonArray blockArray || blockArray.Count < 2) return null;
            return ConvertJsonNodeToString(blockArray[1]);
        }

        private IEnumerable<KeyValuePair<string, TOutput>> ReadBiomes(JsonArray biomeArrayNode, string payloadName, JsonObject? extra)
        {
            List<KeyValuePair<string, TOutput>> output = new(biomeArrayNode.Count);

            foreach (JsonNode? biomeNode in biomeArrayNode) 
            {
                if (biomeNode is null) continue;
                ReadBiome(biomeNode, payloadName, extra, output);
            }

            return output;
        }
        private void ReadBiome(JsonNode biomeNode, string payloadName, JsonObject? extra, IList<KeyValuePair<string, TOutput>> output)
        {
            if (biomeNode is JsonArray biomeArrayNode) ReadBiomeAsArray(biomeArrayNode, extra, output);
            else if (biomeNode is JsonObject biomeObjectNode) ReadBiomeAsObject(biomeObjectNode, payloadName, extra, output);
        }
        private void ReadBiomeAsArray(JsonArray biomeArrayNode, JsonObject? extra, IList<KeyValuePair<string, TOutput>> output)
        {
            string? biome = ConvertJsonNodeToString(biomeArrayNode[0]);
            if (biome is null) return;

            JsonNode? payloadNode = biomeArrayNode[1];
            BiomeReadArgs biomeReadArgs = new(extra, payloadNode, biome);
            TOutput? payload = PayloadReader.Read(biomeReadArgs);
            if (payload is null) return;

            output.Add(new KeyValuePair<string, TOutput>(biome, payload.Value));
        }
        private void ReadBiomeAsObject(JsonObject biomeObjectNode, string payloadName, JsonObject? extra, IList<KeyValuePair<string, TOutput>> output)
        {
            JsonArray? biomeArray = biomeObjectNode["Biomes"]?.AsArray();
            if (biomeArray is null) return;

            JsonNode? payloadNode = biomeObjectNode[payloadName];

            for (int i = 0; i < biomeArray.Count; i++)
            {
                string? biome = ConvertJsonNodeToString(biomeArray[i]);
                if (biome is null) continue;

                BiomeReadArgs biomeReadArgs = new(extra, payloadNode, biome);
                TOutput? payload = PayloadReader.Read(biomeReadArgs);
                if (payload is null) continue;

                output.Add(new KeyValuePair<string, TOutput>(biome, payload.Value));
            }
        }

        private static string? ConvertJsonNodeToString(JsonNode? node)
        {
            if (node is null) return null;
            if (node is not JsonValue value || !value.TryGetValue(out string? output)) return null;

            return output;
        }

        private static void CombineBlocksWithBiomes(IEnumerable<KeyValuePair<WorldEditor.Block, string?>> blocks,
            IEnumerable<KeyValuePair<string, TOutput>> biomes,
            BiomeAsset<TOutput> output)
        {
            foreach (KeyValuePair<WorldEditor.Block, string?> block in blocks)
            {
                foreach (KeyValuePair<string, TOutput> biome in biomes)
                {
                    BiomeBlock key = new(block.Key, biome.Key);

                    PropertyMatcher<TOutput> evaluator;
                    if (block.Value is not null)
                    {
                        evaluator = new(biome.Value, new LogicalExpression(block.Value));
                    }
                    else 
                    {
                        evaluator = new(biome.Value, null);
                    }

                    output.Add(key, evaluator);
                }
            }
        }
    }
}
