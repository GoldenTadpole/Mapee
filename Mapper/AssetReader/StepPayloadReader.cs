using AssetSystem;
using AssetSystem.Block;
using Mapper;
using System.Text.Json.Nodes;

namespace MapScanner
{
    public class StepPayloadReader : IAssetReader<BlockReadArgs, StepType?>
    {
        public StepType? Read(BlockReadArgs input) {
            StepType type = StepType.Ignore;

            if (input.Payload is JsonValue valueNode && valueNode.TryGetValue(out string? value) && value is not null) 
            {
                _ = Enum.TryParse(value, out type);
            }

            return type;
        }
    }
}
