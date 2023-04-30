using System.Text.Json.Nodes;

namespace WorldEditor
{
    public readonly struct BiomeColorReadArgs
    {
        public string TokenName { get; init; }
        public JsonObject ParentToken { get; init; }

        public BiomeColorReadArgs(string tokenName, JsonObject parentToken)
        {
            TokenName = tokenName;
            ParentToken = parentToken;
        }
    }
}
