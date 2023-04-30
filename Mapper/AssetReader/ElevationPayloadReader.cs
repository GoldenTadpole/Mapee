using AssetSystem;
using AssetSystem.Biome;
using AssetSystem.Block;
using System.Text.Json.Nodes;

namespace Mapper {
    public class ElevationPayloadReader : IAssetReader<BlockReadArgs, ElevationSettings?>, IAssetReader<BiomeReadArgs, ElevationSettings?> {
        public ElevationSettings Default { get; set; } = new ElevationSettings();

        public ElevationSettings? Read(BlockReadArgs input) {
            if (input.Payload is null) return Default;
            return LoadFromToken(input.Payload.AsObject());
        }
        public ElevationSettings? Read(BiomeReadArgs input) {
            if (input.Payload is null) return Default;
            return LoadFromToken(input.Payload.AsObject());
        }

        private ElevationSettings LoadFromToken(JsonObject obj) {
            short y = Default.Y, minSteps = Default.MinSteps, maxSteps = Default.MaxSteps;
            float minIncrement = Default.MinIncrement, maxIncrement = Default.MaxIncrement;
            bool multiplyMaxIncrement = Default.MultiplyMaxIncrement;
            VecRgb hue = Default.Hue;

            if (obj.TryGetPropertyValue("Y", out JsonNode? yToken) && yToken is not null) 
            {
                y = yToken.AsValue().GetValue<short>();
            }
            if (obj.TryGetPropertyValue("Min", out JsonNode? minToken) && minToken is not null)
            {
                minSteps = minToken.AsValue().GetValue<short>();
            }
            if (obj.TryGetPropertyValue("MinIncr", out JsonNode? minIncrementToken) && minIncrementToken is not null)
            {
                minIncrement = minIncrementToken.AsValue().GetValue<float>();
            }
            if (obj.TryGetPropertyValue("Max", out JsonNode? maxStepsToken) && maxStepsToken is not null)
            {
                maxSteps = maxStepsToken.AsValue().GetValue<short>();
            }
            if (obj.TryGetPropertyValue("MaxIncr", out JsonNode? maxIncrementToken) && maxIncrementToken is not null)
            {
                maxIncrement = maxIncrementToken.AsValue().GetValue<float>();
            }
            if (obj.TryGetPropertyValue("MultiplyMaxIncrement", out JsonNode? multiplyMaxIncrementToken) && multiplyMaxIncrementToken is not null)
            {
                multiplyMaxIncrement = multiplyMaxIncrementToken.AsValue().GetValue<bool>();
            }
            if (obj.TryGetPropertyValue("Hue", out JsonNode? hueToken) && hueToken is not null)
            {
                hue = LoadColorToken(hueToken.AsArray());
            }

            return new ElevationSettings() {
                Y = y,
                MinSteps = minSteps,
                MinIncrement = minIncrement,
                MaxSteps = maxSteps,
                MaxIncrement = maxIncrement,
                MultiplyMaxIncrement = multiplyMaxIncrement,
                Hue = hue
            };
        }
        private static VecRgb LoadColorToken(JsonArray array) {
            if(array.Count < 3) return new VecRgb();

            float r = array[0]?.AsValue().GetValue<float>() ?? 0;
            float g = array[1]?.AsValue().GetValue<float>() ?? 0;
            float b = array[2]?.AsValue().GetValue<float>() ?? 0;

            return new VecRgb(r, g, b);
        }
    }
}
