using AssetSystem;
using AssetSystem.Block;
using System;
using System.Text.Json.Nodes;

namespace MapScanner
{
    public class BlockGroupingPayloadReader : IAssetReader<BlockReadArgs, BlockGrouping?>
    {
        public BlockGrouping? Read(BlockReadArgs input)
        {
            BlockType type = BlockType.Disabled;
            short exitOnDepth = short.MaxValue;
            string? groupBy = null;

            if (input.DefaultPayload is not null)
            {
                ReadToken(input.DefaultPayload, ref type, ref exitOnDepth, ref groupBy);
            }

            if (input.Payload != input.DefaultPayload && input.Payload is not null)
            {
                ReadToken(input.Payload, ref type, ref exitOnDepth, ref groupBy);
            }

            return new BlockGrouping(type, string.IsNullOrEmpty(groupBy) ? int.MinValue : groupBy.GetHashCode())
            {
                ExitOnDepth = exitOnDepth
            };
        }

        private static void ReadToken(JsonNode token, ref BlockType type, ref short exitOnDepth, ref string? groupBy)
        {
            BlockType? typeOutput = null;
            short? exitOnDepthOutput = null;
            string? groupByOutput = null;

            if (token is JsonValue && token.AsValue().TryGetValue(out string? blockTypeNodeString))
            {
                if (Enum.TryParse(blockTypeNodeString, out BlockType blockType))
                {
                    typeOutput = blockType;
                }
            }
            else if (token is JsonObject obj)
            {
                if (obj.TryGetPropertyValue("BlockType", out JsonNode? blockTypeTag) &&
                    blockTypeTag is not null && blockTypeTag.AsValue().TryGetValue(out blockTypeNodeString))
                {
                    if (Enum.TryParse(blockTypeNodeString, out BlockType blockType))
                    {
                        typeOutput = blockType;
                    }
                }

                if (obj.TryGetPropertyValue("ExitOnDepth", out JsonNode? depthToken) && depthToken is not null) 
                {
                    depthToken.AsValue().TryGetValue(out exitOnDepthOutput);
                }
                if (obj.TryGetPropertyValue("GroupBy", out JsonNode? groupByToken) && groupByToken is not null) 
                {
                    groupByToken.AsValue().TryGetValue(out groupByOutput);
                }
            }

            if (typeOutput is not null) type = typeOutput.Value;
            if (exitOnDepthOutput is not null) exitOnDepth = exitOnDepthOutput.Value;
            if (!string.IsNullOrEmpty(groupByOutput)) groupBy = groupByOutput;
        }
    }
}
