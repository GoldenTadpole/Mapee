using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace WorldEditor
{
    public class CustomBiomeReader : IObjectReader<CustomBiomeReadArgs, CustomBiome?>
    {
        public virtual IObjectReader<BiomeColorReadArgs, IBiomeColor?> BiomeColorReader { get; set; }

        private static readonly string _commentPattern = @"(/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/)|(//.*)";
        private static readonly Regex _commentRemover = new Regex(_commentPattern);

        public CustomBiomeReader()
        {
            BiomeColorReader = new BiomeColorReader();
        }

        public virtual CustomBiome? Read(CustomBiomeReadArgs input)
        {
            string biomeNamespace = Path.GetFileNameWithoutExtension(input.FileName);

            string jsonString = ReadToEnd(input.FileContents);
            jsonString = _commentRemover.Replace(jsonString, string.Empty);

            JsonNode? parsed = JsonNode.Parse(jsonString);
            if (parsed is null || parsed is not JsonObject parentToken) return null;

            string @namespace;
            if (string.IsNullOrEmpty(input.Dimension))
            {
                @namespace = $"{input.Namespace}:{biomeNamespace}";
            }
            else 
            {
                @namespace = $"{input.Namespace}:{input.Dimension}/{biomeNamespace}";
            }

            return new CustomBiome(@namespace)
            {
                GrassColor = BiomeColorReader.Read(new BiomeColorReadArgs("grass_color", parentToken)),
                FolliageColor = BiomeColorReader.Read(new BiomeColorReadArgs("foliage_color", parentToken)),
                WaterColor = BiomeColorReader.Read(new BiomeColorReadArgs("water_color", parentToken)),
            };
        }

        private static string ReadToEnd(byte[] bytes)
        {
            using MemoryStream input = new(bytes);
            using StreamReader reader = new(input);
            return reader.ReadToEnd();
        }
    }
}
