using System.Text.Json.Nodes;

namespace WorldEditor
{
    public class BiomeIdTranslatorReader : IObjectReader<string, BiomeIdTranslator>
    {
        public BiomeIdTranslator Read(string input)
        {
            BiomeIdTranslator output = new();

            JsonArray? array = JsonNode.Parse(input)?.AsArray();
            if (array is null) return output;

            foreach (JsonArray? entry in array.Cast<JsonArray?>())
            {
                if (entry is null || entry is not JsonArray arrayEntry) continue;

                int? key = arrayEntry[0]?.AsValue().GetValue<int>();
                string? value = arrayEntry[1]?.AsValue().GetValue<string>();
                if (key is null || value is null) continue;

                output.IDs.Add(key.Value, value);
            }

            return output;
        }
    }
}
