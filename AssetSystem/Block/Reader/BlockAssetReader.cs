using System.IO;
using System.Text.Json.Nodes;

namespace AssetSystem.Block
{
    public class BlockAssetReader<TOutput> : IAssetReader<AssetArgs, BlockAsset<TOutput>?> where TOutput : struct
    {
        public IAssetReader<BlockAssetPageReadArgs<TOutput>> PageReader { get; set; }

        public BlockAssetReader(IAssetReader<BlockReadArgs, TOutput?> payloadReader)
        {
            PageReader = new BlockAssetPageReader<TOutput>(payloadReader);
        }
        public BlockAssetReader(IAssetReader<BlockAssetPageReadArgs<TOutput>> pageReader)
        {
            PageReader = pageReader;
        }

        public BlockAsset<TOutput>? Read(AssetArgs args)
        {
            if (args.Files is null) return null;
            BlockAsset<TOutput> output = new();

            foreach (string file in args.Files)
            {
                byte[]? bytes = args.Reader.ReadFile(file);
                if (bytes is null) continue;

                string jsonString = ReadStringFromByteArray(bytes);
                JsonArray? pageArray = JsonNode.Parse(jsonString)?.AsArray();
                if (pageArray is null) continue;

                PageReader.Read(new BlockAssetPageReadArgs<TOutput>(pageArray, output));
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
