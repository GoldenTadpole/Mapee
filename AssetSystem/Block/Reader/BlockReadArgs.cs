using System.Text.Json.Nodes;

namespace AssetSystem.Block
{
    public struct BlockReadArgs
    {
        public JsonNode? DefaultPayload { get; set; }
        public JsonNode? Payload { get; set; }
        public string BlockName { get; set; }
        public string? Expression { get; set; }

        public BlockReadArgs(JsonNode? defaultPayload, JsonNode? payload, string blockName, string? expression) 
        {
            DefaultPayload = defaultPayload;
            Payload = payload;
            BlockName = blockName;
            Expression = expression;
        }
        public BlockReadArgs(JsonNode? payload, string blockName, string? expression)
        {
            Payload = payload;
            BlockName = blockName;
            Expression = expression;
        }
    }
}
