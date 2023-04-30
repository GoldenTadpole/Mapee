using AssetSystem;
using AssetSystem.Biome;
using CommonUtilities.Factory;
using System.Text.Json.Nodes;
using System.Windows.Media;

namespace Mapper
{
    public class VecRgbBiomePayloadReader : IAssetReader<BiomeReadArgs, VecRgb?> 
    {
        public string TextureFolder { get; set; }
        public IFactory<string, IReadOnlyBitmap?>? LockedBitmapFactory { get; set; }

        private IDictionary<string, IReadOnlyBitmap> _bitmapCache;

        public VecRgbBiomePayloadReader(string textureFolder, IFactory<string, IReadOnlyBitmap?>? lockedBitmapFactory)
        {
            TextureFolder = textureFolder;
            LockedBitmapFactory = lockedBitmapFactory;
            _bitmapCache = new Dictionary<string, IReadOnlyBitmap>();
        }

        public VecRgb? Read(BiomeReadArgs input) 
        {
            if (input.Extra is null || input.Payload is null) return VecRgb.Empty;

            JsonNode? typeNode = input.Extra["Type"];
            if(typeNode is null) return VecRgb.Empty;

            string type = typeNode.AsValue().GetValue<string>();
            switch (type)
            {
                case "Colormap":
                    return GetPixel(input.Extra, input.Payload.AsArray());
                case "HardCoded":
                    return GetColor(input.Payload);
                case "Overlayed":
                    JsonNode? payload = input.Payload[0];
                    if (payload is null) return VecRgb.Empty;

                    VecRgb? pixel = GetPixel(input.Extra, payload.AsArray());
                    if (pixel is null) return null;
                    
                    VecRgb color = GetColor(input.Payload[1]);

                    return (pixel + color) / 2;
            }

            return 0;
        }

        private VecRgb? GetPixel(JsonNode header, JsonArray pixelArray)
        {
            if (pixelArray.Count < 2) return null;

            JsonNode? colormap = header["Colormap"];
            if(colormap is null) return null;

            int x = pixelArray[0]?.AsValue().GetValue<int>() ?? 0;
            int y = pixelArray[1]?.AsValue().GetValue<int>() ?? 0;

            return GetBitmap(colormap.AsValue().GetValue<string>())?.GetPixel(x, y) ?? null;
        }
        private VecRgb GetColor(JsonNode? token)
        {
            if(token is null) return VecRgb.Empty;
            return (Color) ColorConverter.ConvertFromString(token.AsValue().GetValue<string>());
        }

        private IReadOnlyBitmap? GetBitmap(string bitmapPath)
        {
            if(LockedBitmapFactory is null) return null;

            string path = $"{TextureFolder}\\{bitmapPath}";
            if (!_bitmapCache.TryGetValue(path, out IReadOnlyBitmap? bitmap))
            {
                bitmap = LockedBitmapFactory.Create(path);
                if (bitmap is null) return null;

                _bitmapCache.Add(path, bitmap);
            }

            return bitmap;
        }
    }
}
