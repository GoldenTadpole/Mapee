using AssetSystem;
using AssetSystem.Block;
using System.Text.Json.Nodes;

namespace Mapper
{
    public class StepSettingsPayloadReader : IAssetReader<BlockReadArgs, StepSettings?> 
    {
        public StepSettings Default { get; set; } = new StepSettings();

        public StepSettings? Read(BlockReadArgs input) {
            StepSettings output = Default;

            JsonObject? obj = input.Payload as JsonObject;
            if (obj is null) return output;

            output.ZNegCorner = ReadCornerToken(obj, "ZNegCorner", Default.ZNegCorner);
            output.XPosCorner = ReadCornerToken(obj, "XPosCorner", Default.XPosCorner);
            output.ZPosCorner = ReadCornerToken(obj, "ZPosCorner", Default.ZPosCorner);
            output.XNegCorner = ReadCornerToken(obj, "XNegCorner", Default.XNegCorner);

            output.BelowTotalLimit = ReadLimitToken(obj, "BelowTotalLimit", Default.BelowTotalLimit);
            output.AboveTotalLimit = ReadLimitToken(obj, "AboveTotalLimit", Default.AboveTotalLimit);

            if (obj.TryGetPropertyValue("Increment", out JsonNode? incrementToken) && incrementToken is not null) 
            {
                output.Increment = incrementToken.AsValue().GetValue<float>();
            }

            return output;
        }

        private static StepCornerSettings ReadCornerToken(JsonObject obj, string name, StepCornerSettings defaultCorner) {
            Limit belowLimit = defaultCorner.BelowLimit;
            float belowIncrement = defaultCorner.BelowIncrement;

            Limit aboveLimit = defaultCorner.AboveLimit;
            float aboveIncrement = defaultCorner.AboveIncrement;

            belowLimit = ReadLimitToken(obj, $"{name}.BelowLimit", belowLimit);
            if (obj.TryGetPropertyValue($"{name}.BelowIncrement", out JsonNode? belowIncrementToken) && belowIncrementToken is not null) 
            {
                belowIncrement = belowIncrementToken.AsValue().GetValue<float>();
            }

            aboveLimit = ReadLimitToken(obj, $"{name}.AboveLimit", aboveLimit);
            if (obj.TryGetPropertyValue($"{name}.AboveIncrement", out JsonNode? aboveIncrementToken) && aboveIncrementToken is not null) 
            {
                aboveIncrement = aboveIncrementToken.AsValue().GetValue<float>();
            }

            return new StepCornerSettings() { 
                BelowLimit = belowLimit,
                BelowIncrement = belowIncrement,
                AboveLimit = aboveLimit,
                AboveIncrement = aboveIncrement
            };
        }
        private static Limit ReadLimitToken(JsonObject obj, string name, Limit defaultLimit) {
            short max = defaultLimit.Max;
            float maxReturnedValue = defaultLimit.MaxReturnedValue;

            short min = defaultLimit.Min;
            float minReturnedValue = defaultLimit.MinReturnedValue;

            if (obj.TryGetPropertyValue($"{name}.Max", out JsonNode? maxToken) && maxToken is not null) 
            {
                max = maxToken.AsValue().GetValue<short>();
            }
            if (obj.TryGetPropertyValue($"{name}.MaxReturnedValue", out JsonNode? maxReturnedValueToken) && maxReturnedValueToken is not null)
            {
                maxReturnedValue = maxReturnedValueToken.AsValue().GetValue<float>();
            }
            if (obj.TryGetPropertyValue($"{name}.Min", out JsonNode? minToken) && minToken is not null)
            {
                min = minToken.AsValue().GetValue<short>();
            }
            if (obj.TryGetPropertyValue($"{name}.MinReturnedValue", out JsonNode? minReturnedValueToken) && minReturnedValueToken is not null)
            {
                minReturnedValue = minReturnedValueToken.AsValue().GetValue<float>();
            }

            return new Limit(max, maxReturnedValue, min, minReturnedValue);
        }
    }
}
