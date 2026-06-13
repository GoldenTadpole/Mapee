using System.Text.Json.Nodes;

namespace WorldEditor
{
    public class BiomeRenamerReader : IObjectReader<string, BiomeRenamer>
    {
        public BiomeRenamer Read(string input)
        {
            BiomeRenamer output = new();

            JsonArray? jsonArray = JsonNode.Parse(input)?.AsArray();
            if (jsonArray is null) return output;

            foreach (JsonNode? timelineNode in jsonArray)
            {
                if (timelineNode is not JsonArray timelineArray) continue;

                BiomeTimeline timeline = ParseTimeline(timelineArray);
                if (timeline.Biomes.Count == 0) continue;

                output.AddTimeline(timeline);
            }

            return output;
        }

        private static BiomeTimeline ParseTimeline(JsonArray timelineArray)
        {
            BiomeTimeline timeline = new();

            foreach (JsonNode? biomeNode in timelineArray)
            {
                if (biomeNode is not JsonObject biomeObj) continue;

                int? dataVersion = biomeObj["DataVersion"]?.GetValue<int>();
                string? biomeName = biomeObj["Biome"]?.GetValue<string>();

                if (dataVersion is null || string.IsNullOrEmpty(biomeName)) continue;

                timeline.Biomes.Add(((Version)dataVersion.Value, biomeName));
            }

            return timeline;
        }
    }
}
