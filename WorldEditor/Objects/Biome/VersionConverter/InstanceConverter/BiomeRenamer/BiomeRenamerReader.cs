using System.Text.Json.Nodes;

namespace WorldEditor
{
    public class BiomeRenamerReader : IObjectReader<string, BiomeRenamer>
    {
        public BiomeRenamer Read(string input)
        {
            BiomeRenamer output = new();

            JsonArray? array = JsonNode.Parse(input)?.AsArray();
            if (array is null) return output;

            foreach (JsonArray? entry in array.Cast<JsonArray?>())
            {
                if (entry is null || entry is not JsonArray arrayEntry) continue;

                string? key = arrayEntry[0]?.AsValue().GetValue<string>();
                string? value = arrayEntry[1]?.AsValue().GetValue<string>();
                if (key is null || value is null) continue;

                output.Biomes.Add(key, value);
            }

            return output;
        }
    }
}
