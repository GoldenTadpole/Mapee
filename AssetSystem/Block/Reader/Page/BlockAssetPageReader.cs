using System.Text.Json.Nodes;

namespace AssetSystem.Block
{
    public class BlockAssetPageReader<TOutput> : IAssetReader<BlockAssetPageReadArgs<TOutput>> where TOutput : struct
    {
        public IAssetReader<BlockReadArgs, TOutput?> PayloadReader { get; set; }

        public BlockAssetPageReader(IAssetReader<BlockReadArgs, TOutput?> payloadReader)
        {
            PayloadReader = payloadReader;
        }

        public void Read(BlockAssetPageReadArgs<TOutput> input)
        {
            JsonArray pageArray = input.PageNode;
            if (pageArray.Count < 1) return;

            JsonNode? defaultPayload = GetDefaulyPayload(pageArray);

            foreach(JsonNode? blockNode in pageArray)
            {
                if (blockNode is null) continue;
                ReadBlock(blockNode, defaultPayload, input.Output);
            }
        }
        private static JsonNode? GetDefaulyPayload(JsonArray pageArray)
        {
            if (pageArray[0] is not JsonObject payloadObject) return null;
            return payloadObject["DefaultPayload"];
        }

        private void ReadBlock(JsonNode blockNode, JsonNode? defaultPaylaod, BlockAsset<TOutput> output) 
        {
            if (blockNode is JsonValue blockValue)
            {
                ReadBlockAsValue(blockValue, defaultPaylaod, output);
                return;
            }

            if(blockNode is JsonArray blockArray) 
            {
                ReadBlockAsArray(blockArray, defaultPaylaod, output);
            }
        }
        private void ReadBlockAsValue(JsonValue? blockValue, JsonNode? defaultPaylaod, BlockAsset<TOutput> output) 
        {
            if (blockValue is null || defaultPaylaod is null) return;
            if(!blockValue.TryGetValue(out string? blockName)) return;

            AddToAsset(blockName,
                null,
                PayloadReader.Read(new BlockReadArgs(defaultPaylaod, null, blockName, null)),
                output);
        }
        private void ReadBlockAsArray(JsonArray blockArray, JsonNode? defaultPaylaod, BlockAsset<TOutput> output) 
        {
            if (blockArray.Count < 1) return;
            if (blockArray.Count == 1)
            {
                ReadBlockAsValue(blockArray[0]?.AsValue(), defaultPaylaod, output);
                return;   
            }

            string? blockName = GetBlockName(blockArray[0]);
            if (blockName is null) return;

            bool containsProperty = BlockContainsProperty(blockName);
            if (!containsProperty)
            {
                AddToAsset(blockName,
                    null,
                    PayloadReader.Read(new BlockReadArgs(defaultPaylaod, blockArray[1], blockName, null)),
                    output);
            }
            else 
            {
                ReadBlockWithProperty(blockName, blockArray, defaultPaylaod, output);
            }
        }
        private void ReadBlockWithProperty(string blockName, JsonArray blockArray, JsonNode? defaultPaylaod, BlockAsset<TOutput> output) 
        {
            if (blockArray[1] is not JsonArray propertyArray)
            {
                ReadBlockWithSingleProperty(blockName, blockArray, defaultPaylaod, output);
            }
            else 
            {
                ReadBlockWithMultipleProperties(blockName, blockArray, propertyArray, defaultPaylaod, output);
            }
        }
        private void ReadBlockWithSingleProperty(string blockName, JsonArray blockArray, JsonNode? defaultPaylaod, BlockAsset<TOutput> output) 
        {
            string? expression = GetExpression(blockArray[1]);
            if (expression is null) return;

            JsonNode? payload;
            if (blockArray.Count == 2) payload = defaultPaylaod;
            else
            {
                payload = blockArray[2];
            }

            string blockNameNoSuffix = ConvertToNameWithoutSuffix(blockName);
            AddToAsset(blockNameNoSuffix,
                expression,
                PayloadReader.Read(new BlockReadArgs(defaultPaylaod, payload, blockName, expression)),
                output);
        }
        private void ReadBlockWithMultipleProperties(string blockName, JsonArray blockArray, JsonArray propertyArray, JsonNode? defaultPayloadNode, BlockAsset<TOutput> output) 
        {
            string blockNameNoSuffix = ConvertToNameWithoutSuffix(blockName);

            foreach (JsonNode? propertyNode in propertyArray) 
            {
                if (propertyNode is null || propertyNode is not JsonArray property || property.Count < 2) continue;

                string? expression = GetExpression(property[0]);
                if (expression is null) continue;

                JsonNode? payload = property[1];
                if (payload is null) continue;

                AddToAsset(blockNameNoSuffix,
                    expression,
                    PayloadReader.Read(new BlockReadArgs(defaultPayloadNode, payload, blockName, expression)),
                    output);
            }

            TOutput? defaultPayload = GetDefaultPropertyValue(blockNameNoSuffix, blockArray, defaultPayloadNode);
            if (defaultPayload is null) return;

            if (output.Blocks.TryGetValue(blockNameNoSuffix, out BlockEntry<TOutput>? blockEntry) &&
                blockEntry is not null) 
            {
                blockEntry.DefaultValue = defaultPayload;
            }
        }
        private TOutput? GetDefaultPropertyValue(string blockName, JsonArray blockArray, JsonNode? defaultPaylaod)
        {
            JsonNode? lastNode = blockArray[blockArray.Count - 1];
            if (lastNode is null || lastNode is not JsonObject objectNode) return null;

            JsonNode? payload = objectNode["Default"];
            if(payload is null) return null;

            return PayloadReader.Read(new BlockReadArgs(defaultPaylaod, payload, blockName, null));
        }

        private static string? GetBlockName(JsonNode? node) 
        {
            if (node is null) return null;
            JsonValue value = node.AsValue();

            if (value.TryGetValue(out string? blockName)) return blockName;
            return null;
        }
        private static string? GetExpression(JsonNode? node)
        {
            if (node is null || node is not JsonValue value) return null;
            if (!value.TryGetValue(out string? expression)) return null;

            return expression;
        }

        private static bool BlockContainsProperty(string name) 
        {
            return name.EndsWith("*");
        }
        private static string ConvertToNameWithoutSuffix(string name) 
        {
            return name[..^1];
        }

        private static void AddToAsset(string blockName, string? expression, TOutput? value, BlockAsset<TOutput> output) 
        {
            if (value is null) return;

            if (expression is not null)
            {
                if (!output.Blocks.TryGetValue(blockName, out BlockEntry<TOutput>? entry))
                {
                    entry = new(blockName);
                    output.Blocks.Add(blockName, entry);
                }

                entry.Evaluators.Add(new PropertyMatcher<TOutput>(value.Value, new LogicalExpression(expression)));
            }
            else 
            {
                if (!output.Blocks.ContainsKey(blockName))
                {
                    output.Blocks.Add(blockName, new(blockName)
                    {
                        DefaultValue = value,
                    });
                }
            }
        }
    }
}
