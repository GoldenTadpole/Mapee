using System.Text.Json.Nodes;

namespace WorldEditor
{
    public class BiomeIdTranslatorReader : IObjectReader<string, BiomeIdTranslator>
    {
        public BiomeIdTranslator Read(string input)
        {
            BiomeIdTranslator output = new();

            JsonArray? jsonArray = JsonNode.Parse(input)?.AsArray();
            if (jsonArray == null) return output;

            foreach (JsonNode? node in jsonArray)
            {
                if (node == null) continue;

                JsonObject? obj = node.AsObject();
                if (obj == null) continue;

                int? id = obj["Id"]?.GetValue<int>();
                string? biome = obj["Biome"]?.GetValue<string>();

                if (id.HasValue && !string.IsNullOrEmpty(biome))
                {
                    output.IDs[id.Value] = biome;
                }
            }

            return output;
        }
    }
}
