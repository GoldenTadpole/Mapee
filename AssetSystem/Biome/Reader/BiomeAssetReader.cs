using AssetSystem.Biome.Reader;
using System.IO;
using System.Text.Json.Nodes;

namespace AssetSystem.Biome
{
    public class BiomeAssetReader<TOutput> : IAssetReader<AssetArgs, BiomeAsset<TOutput>?> where TOutput : struct
    {
        public IAssetReader<BiomeAssetPageReadArgs<TOutput>> PageReader { get; set; }

        public BiomeAssetReader(IAssetReader<BiomeReadArgs, TOutput?> payloadReader) 
        {
            PageReader = new BiomeAssetPageReader<TOutput>(payloadReader);
        }
        public BiomeAssetReader(IAssetReader<BiomeAssetPageReadArgs<TOutput>> pageReader)
        {
            PageReader = pageReader;
        }

        public BiomeAsset<TOutput>? Read(AssetArgs args)
        {
            if (args.Files is null) return null;

            BiomeAsset<TOutput> output = new();
            foreach (string file in args.Files)
            {
                byte[]? bytes = args.Reader.ReadFile(file);
                if (bytes is null) continue;

                string jsonString = ReadStringFromByteArray(bytes);
                JsonObject? pageObject = JsonNode.Parse(jsonString)?.AsObject();
                if (pageObject is null) continue;

                PageReader.Read(new BiomeAssetPageReadArgs<TOutput>(pageObject, output));
            }

            return output;
        }

        private static string ReadStringFromByteArray(byte[] array)
        {
            using MemoryStream input = new MemoryStream(array);
            using StreamReader reader = new StreamReader(input);
            return reader.ReadToEnd();
        }
    }
}
