using System.Text.Json.Nodes;

namespace Mapper
{
    public readonly struct BlockAssetArgs
    {
        public JsonNode Args { get; init; }
        public string? Source { get; init; }
        public JsonNode? DefaultOutput { get; init; }
        public bool DefaultOutputExists { get; init; }

        public BlockAssetArgs(JsonNode args, string? source, JsonNode? defaultOutput) 
        {
            Args = args;
            Source = source;
            DefaultOutput = defaultOutput;
        }
    }
}
