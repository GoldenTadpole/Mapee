using AssetSystem;
using AssetSystem.Block;
using CommonUtilities.Factory;
using System.Text.Json.Nodes;
using System.Windows.Media;

namespace Mapper
{
    public class RgbAPayloadReader : IAssetReader<BlockReadArgs, RgbA?>
    {
        public string TextureFolder { get; set; }
        public IFactory<string, IReadOnlyBitmap?>? LockedBitmapFactory { get; set; }

        private IDictionary<string, IReadOnlyBitmap> _bitmapCache;
        private IDictionary<string, RgbA?> _namespaceCache;

        public RgbAPayloadReader(string textureFolder, IFactory<string, IReadOnlyBitmap?>? lockedBitmapFactory)
        {
            TextureFolder = textureFolder;
            LockedBitmapFactory = lockedBitmapFactory;

            _bitmapCache = new Dictionary<string, IReadOnlyBitmap>();
            _namespaceCache = new Dictionary<string, RgbA?>();
        }

        public RgbA? Read(BlockReadArgs input)
        {
            RgbA? output;

            if (input.Payload is JsonValue valueNode && valueNode.TryGetValue(out string _))
            {
                RgbA? nullableOutput = ReadStringToken(input.Payload);
                if (nullableOutput is null) return null;

                output = nullableOutput.Value;
            }
            else if (input.Payload is JsonArray arrayNode)
            {
                if (arrayNode.Count > 0 && arrayNode[0] is JsonValue value && value.TryGetValue(out float _))
                {
                    output = ReadRgbArrayToken(arrayNode);
                }
                else
                {
                    output = ReadArrayToken(input, arrayNode);
                }
            }
            else
            {
                if (input.Payload is null) return null;
                output = ReadObjectToken(input, input.Payload.AsObject(), null, null);
            }

            if (!_namespaceCache.ContainsKey(input.BlockName))
            {
                _namespaceCache.Add(input.BlockName, output);
            }

            return output;
        }

        private RgbA? ReadStringToken(JsonNode token)
        {
            string value = token.AsValue().GetValue<string>();
            if (value.StartsWith("#")) 
            {
                Color color = (Color)ColorConverter.ConvertFromString(value);
                return new RgbA(color, color.A / 255F);
            }

            if (LockedBitmapFactory is null) return null;

            string path = $"{TextureFolder}\\{value}";
            if (!_bitmapCache.TryGetValue(path, out IReadOnlyBitmap? bitmap))
            {
                bitmap = LockedBitmapFactory.Create(path);
                if (bitmap is null) return null;

                _bitmapCache.Add(path, bitmap);
            }

            Color averageColor = bitmap.GetAverageColor();
            return new RgbA(averageColor, averageColor.A / 255F);
        }
        private static RgbA ReadRgbArrayToken(JsonArray arrayToken)
        {
            RgbA color;

            if (arrayToken.Count == 3)
            {
                color = new RgbA(new VecRgb(
                    arrayToken[0]?.GetValue<float>() ?? 0,
                    arrayToken[1]?.GetValue<float>() ?? 0,
                    arrayToken[2]?.GetValue<float>() ?? 0));
            }
            else
            {
                color = new RgbA(new VecRgb(
                    arrayToken[0]?.GetValue<float>() ?? 0,
                    arrayToken[1]?.GetValue<float>() ?? 0,
                    arrayToken[2]?.GetValue<float>() ?? 0),
                    arrayToken[3]?.GetValue<float>() ?? 0);
            }

            return color;
        }
        private RgbA? ReadArrayToken(BlockReadArgs parameter, JsonArray arrayToken)
        {
            JsonObject? objToken = arrayToken[1] as JsonObject;
            if (objToken is null) return null;

            return ReadObjectToken(parameter, objToken, arrayToken[0], Read(new BlockReadArgs(arrayToken[0], parameter.BlockName, parameter.Expression)));
        }
        private RgbA? ReadObjectToken(BlockReadArgs parameter, JsonObject obj, JsonNode? originalToken, RgbA? input)
        {
            if (obj.ContainsKey("MixIntensity"))
            {
                input = ReadMixIntensityObjectToken(parameter, obj, input);
            }
            else if (obj.ContainsKey("Area") && originalToken is not null)
            {
                input = ReadAreaObjectToken(originalToken, obj);
            }
            else if (obj.ContainsKey("Overlay"))
            {
                input = ReadOverlayObjectToken(parameter, obj, input);
            }
            else if (obj.ContainsKey("Copy"))
            {
                input = ReadCopyObjectToken(obj);
            }

            if (input == null)
            {
                input = Read(new BlockReadArgs(obj["Main"], parameter.BlockName, parameter.Expression));
            }

            if (input is null) return null;

            if (obj.ContainsKey("OpacitySet"))
            {
                JsonNode? opacitySetNode = obj["OpacitySet"];
                if (opacitySetNode is null) return input.Value;

                input = input?.SetOpacity(opacitySetNode.AsValue().GetValue<float>());
            }
            else if (obj.ContainsKey("OpacityMul"))
            {
                JsonNode? opacityMulNode = obj["OpacityMul"];
                if (opacityMulNode is null) return input.Value;

                input = input?.SetOpacity(input.Value.A * opacityMulNode.AsValue().GetValue<float>());
            }

            return input;
        }

        private RgbA? ReadMixIntensityObjectToken(BlockReadArgs parameter, JsonObject obj, RgbA? input)
        {
            if (input == null || obj.ContainsKey("Main"))
            {
                input = Read(new BlockReadArgs(obj["Main"], parameter.BlockName, parameter.Expression));
                if (input is null) return null;
            }
            
            RgbA? other = Read(new BlockReadArgs(obj["Other"], parameter.BlockName, parameter.Expression));
            if (other is null) return null;

            JsonNode? mixIntensityNode = obj["MixIntensity"];
            if (mixIntensityNode is null) return RgbA.Empty;

            float intensity = mixIntensityNode.AsValue().GetValue<float>();
            return new RgbA(input.Value.Rgb.Mix(other.Value.Rgb, intensity), (input.Value.A - other.Value.A) * intensity + other.Value.A);
        }
        private RgbA? ReadAreaObjectToken(JsonNode originalToken, JsonObject obj)
        {
            if (LockedBitmapFactory is null) return null;

            string path = $"{TextureFolder}\\{originalToken.AsValue().GetValue<string>()}";
            if (!_bitmapCache.TryGetValue(path, out IReadOnlyBitmap? bitmap))
            {
                bitmap = LockedBitmapFactory.Create(path);
                if (bitmap is null) return null;

                _bitmapCache.Add(path, bitmap);
            }

            JsonArray? arrayToken = obj["Area"] as JsonArray;
            if(arrayToken is null) return null;

            return new RgbA(bitmap.GetAverageColor(
                arrayToken[0]?.GetValue<int>() ?? 0,
                arrayToken[1]?.GetValue<int>() ?? 0,
                arrayToken[2]?.GetValue<int>() ?? 0,
                arrayToken[3]?.GetValue<int>() ?? 0));
        }
        private RgbA? ReadOverlayObjectToken(BlockReadArgs parameter, JsonObject obj, RgbA? input)
        {
            if (input is null || obj.ContainsKey("Main"))
            {
                input = Read(new BlockReadArgs(obj["Main"], parameter.BlockName, parameter.Expression));
                if (input is null) return null;
            }

            RgbA? other = Read(new BlockReadArgs(obj["Overlay"], parameter.BlockName, parameter.Expression));
            if (other is null) return null;

            return new RgbA((input.Value.Rgb * other.Value.Rgb).Clamp(), input.Value.A);
        }
        private RgbA? ReadCopyObjectToken(JsonObject obj)
        {
            JsonNode? copyNode = obj["Copy"];
            if (copyNode is null) return null;

            return _namespaceCache[copyNode.AsValue().GetValue<string>()];
        }
    }
}
