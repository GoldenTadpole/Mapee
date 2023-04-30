using System.Text.Json.Nodes;

namespace AssetSystem.Biome
{
    public struct BiomeReadArgs
    {
        public JsonNode? Extra { get; set; }
        public JsonNode? Payload { get; set; }
        public string BiomeName { get; set; }

        public BiomeReadArgs(JsonNode? extra, JsonNode? payload, string biomeName) {
            Extra = extra;
            Payload = payload;
            BiomeName = biomeName;
        }
    }
}
