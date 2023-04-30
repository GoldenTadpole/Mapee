using System.IO;
using System.Text.Json.Nodes;
using WorldEditor;

namespace Mapper
{
    public class AssetInstructionsReader : IObjectReader<byte[]?, AssetInstructions?>
    {
        public AssetInstructions? Read(byte[]? input)
        {
            if (input is null) return null;

            string jsonString = ReadToEnd(input);
            JsonObject? obj = JsonNode.Parse(jsonString)?.AsObject();
            if(obj is null) return null;

            return new AssetInstructions() 
            {
                BlockGrouping = ReadBlockAssetArgs(obj["BlockGrouping"]?.AsObject()),
                BlockColor = ReadBlockAssetArgs(obj["BlockColor"]?.AsObject()),
                BiomeColor = ReadBiomeAssetArgs(obj["BiomeColor"]?.AsObject()),
                Elevation = ReadBiomeAssetArgs(obj["Elevation"]?.AsObject()),
                DepthOpacity = ReadBiomeAssetArgs(obj["DepthOpacity"]?.AsObject()),
                Step = ReadBlockAssetArgs(obj["Step"]?.AsObject()),
                StepSettings = ReadBlockAssetArgs(obj["StepSettings"]?.AsObject())
            };
        }

        private static string ReadToEnd(byte[] bytes)
        {
            using MemoryStream input = new(bytes);
            using StreamReader reader = new(input);
            return reader.ReadToEnd();
        }

        private static BlockAssetArgs ReadBlockAssetArgs(JsonObject? obj)
        {
            if (obj is null) return new BlockAssetArgs(); 

            string? source = obj["Source"]?.AsValue()?.GetValue<string>();
            JsonNode? defaultOutput = obj["DefaultOutput"];

            return new BlockAssetArgs(obj, source, defaultOutput) 
            {
                DefaultOutputExists = obj.ContainsKey("DefaultOutput")
            };
        }
        private static BiomeAssetArgs ReadBiomeAssetArgs(JsonObject? obj)
        {
            if (obj is null) return new BiomeAssetArgs();

            BlockAssetArgs args = ReadBlockAssetArgs(obj);
            string? defaultBiome = obj["DefaultBiome"]?.AsValue()?.GetValue<string>();

            return new BiomeAssetArgs(args, defaultBiome) 
            {
                DefaultBiomeExists = obj.ContainsKey("DefaultBiome")
            };
        }
    }
}
