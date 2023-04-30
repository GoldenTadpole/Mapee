using AssetSystem;
using AssetSystem.Biome;
using AssetSystem.Block;
using System.Text.Json.Nodes;

namespace Mapper 
{
    public class DepthOpacityPayloadReader : IAssetReader<BlockReadArgs, DepthOpacity?>, IAssetReader<BiomeReadArgs, DepthOpacity?>
    {
        public DepthOpacity Default { get; set; }

        public DepthOpacity? Read(BlockReadArgs input)
        {
            if (input.Payload is null) return Default;
            return ReadToken(input.Payload.AsObject());
        }
        public DepthOpacity? Read(BiomeReadArgs input)
        {
            if (input.Payload is null) return Default;
            return ReadToken(input.Payload.AsObject());
        }

        private static DepthOpacity ReadToken(JsonObject obj)
        {
            float opacity = 1;
            OpacityType opacityType = OpacityType.Multiply;
            float maxDepth = 1;
            int id = int.MinValue;

            if (obj.TryGetPropertyValue("OpacitySet", out JsonNode? opacityNode) && opacityNode is not null) 
            {
                opacity = opacityNode.AsValue().GetValue<float>();
                opacityType = OpacityType.Set;
            } 
            else if (obj.TryGetPropertyValue("OpacityMul", out JsonNode? opacityMulNode) && opacityMulNode is not null) 
            {
                opacity = opacityMulNode.AsValue().GetValue<float>();
                opacityType = OpacityType.Multiply;
            }
            else if (obj.TryGetPropertyValue("DecraseTransparencyMul", out JsonNode? transparencyNode) && transparencyNode is not null)
            {
                opacity = transparencyNode.AsValue().GetValue<float>();
                opacityType = OpacityType.DecraseTransparency;
            }

            if (obj.TryGetPropertyValue("MaxDepth", out JsonNode? maxDepthNode) && maxDepthNode is not null)
            {
                maxDepth = maxDepthNode.AsValue().GetValue<float>();
            }
            if (obj.TryGetPropertyValue("Id", out JsonNode? idNode) && idNode is not null)
            {
                id = idNode.AsValue().GetValue<string>().GetHashCode();
            }

            return new DepthOpacity(opacity, opacityType, maxDepth, id);
        }
    }
}
