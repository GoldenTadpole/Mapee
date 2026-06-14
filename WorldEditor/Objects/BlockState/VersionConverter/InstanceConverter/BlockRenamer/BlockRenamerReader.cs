using System.Text.Json.Nodes;

namespace WorldEditor
{
    public class BlockRenamerReader : IObjectReader<string, BlockRenamer>
    {
        public BlockRenamer Read(string jsonString)
        {
            BlockRenamer output = new();

            JsonArray? jsonArray = JsonNode.Parse(jsonString)?.AsArray();
            if (jsonArray is null) return output;

            foreach (JsonNode? timelineNode in jsonArray)
            {
                if (timelineNode is not JsonArray timelineArray) continue;

                BlockTimeline timeline = ParseTimeline(timelineArray);
                if (timeline.Blocks.Count == 0) continue;

                output.AddTimeline(timeline);
            }

            return output;
        }

        private static BlockTimeline ParseTimeline(JsonArray timelineArray)
        {
            BlockTimeline timeline = new();

            foreach (JsonNode? blockNode in timelineArray)
            {
                if (blockNode is not JsonObject blockObj) continue;

                int? dataVersion = blockObj["DataVersion"]?.GetValue<int>();
                string? blockString = blockObj["Block"]?.GetValue<string>();

                if (dataVersion is null || blockString is null) continue;

                Block block = ParseBlock(blockString);
                if (block.IsEmpty()) continue;

                timeline.Blocks.Add(((Version)dataVersion.Value, block));
            }

            return timeline;
        }

        private static Block ParseBlock(string blockString)
        {
            if (string.IsNullOrEmpty(blockString)) return Block.Empty;

            string[] parts = blockString.Split(' ', 2);
            string name = parts[0];

            if (parts.Length == 1)
            {
                return new Block(name);
            }

            string[] propertyPairs = parts[1].Split(';');
            List<Property> properties = new(propertyPairs.Length);

            foreach (string pair in propertyPairs)
            {
                string[] keyValue = pair.Split('=', 2);
                if (keyValue.Length != 2 || string.IsNullOrEmpty(keyValue[0]) || string.IsNullOrEmpty(keyValue[1]))
                {
                    continue;
                }

                properties.Add(new Property(keyValue[0], keyValue[1]));
            }

            Property[] sortedProperties = properties.ToArray();
            Array.Sort(sortedProperties, (a, b) => string.CompareOrdinal(a.Name, b.Name));

            return new Block(name, sortedProperties);
        }
    }
}
