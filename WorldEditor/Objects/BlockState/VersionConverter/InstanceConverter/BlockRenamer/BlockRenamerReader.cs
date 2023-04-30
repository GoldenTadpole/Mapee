using System.Text.Json.Nodes;

namespace WorldEditor
{
    public class BlockRenamerReader : IObjectReader<string, BlockRenamer>
    {
        public BlockRenamer Read(string jsonString)
        {
            BlockRenamer output = new();

            JsonArray? array = JsonNode.Parse(jsonString)?.AsArray();
            if (array is null) return output;

            foreach (JsonNode? entry in array)
            {
                if (entry is not JsonArray blockEntry) continue;

                JsonArray? oldBlockArray = blockEntry[0]?.AsArray();
                if (oldBlockArray is null) continue;

                JsonArray? newBlockArray = blockEntry[1]?.AsArray();
                if (newBlockArray is null) continue;

                Block? oldBlock = ReadBlock(oldBlockArray);
                Block? newBlock = ReadBlock(newBlockArray);

                if(oldBlock is null || newBlock is null) continue;

                if (!output.Blocks.TryGetValue(oldBlock.Value.Name, out RenamedBlock? renamedBlock))
                {
                    renamedBlock = new RenamedBlock(oldBlock.Value.Name);
                    output.Blocks.Add(renamedBlock.OldNamespace, renamedBlock);
                }

                renamedBlock.OldBlocks.Add(oldBlock.Value);
                renamedBlock.NewBlocks.Add(newBlock.Value);
            }

            return output;
        }

        private static Block? ReadBlock(JsonArray array)
        {
            JsonArray? propertiesArray = array[1]?.AsArray();
            if(propertiesArray is null) return null;

            Property[] properties = new Property[propertiesArray.Count];
            for (int i = 0; i < properties.Length; i++)
            {
                if (propertiesArray[i] is not JsonArray propertyArray) continue;

                Property? property = ReadProperty(propertyArray);
                if (property is null) continue;

                properties[i] = property.Value;
            }

            string? name = array[0]?.AsValue().GetValue<string>();
            if(name is null) return null;

            return new Block(name, properties);
        }

        private static Property? ReadProperty(JsonArray propertyTag)
        {
            string? name = propertyTag[0]?.AsValue().GetValue<string>();
            string? value = propertyTag[1]?.AsValue().GetValue<string>();

            if (name is null || value is null) return null;

            return new Property(name, value);
        }
    }
}
