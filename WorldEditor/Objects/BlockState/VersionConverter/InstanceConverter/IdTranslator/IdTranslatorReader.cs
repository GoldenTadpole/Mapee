using System.Text.Json.Nodes;

namespace WorldEditor
{
    public class IdTranslatorReader : IObjectReader<string, IdTranslator>
    {
        public IdTranslator Read(string jsonString)
        {
            IdTranslator output = new();

            JsonArray? array = JsonNode.Parse(jsonString)?.AsArray();
            if (array is null) return output;

            foreach (JsonNode? block in array)
            {
                if (block is not JsonArray blockArray) continue;

                byte? blockState = blockArray[0]?.AsValue().GetValue<byte>();
                byte? data = blockArray[1]?.AsValue().GetValue<byte>();
                string? name = blockArray[2]?.AsValue().GetValue<string>();

                if (blockState is null || data is null || name is null) continue;
                if (block[3] is null || block[3] is not JsonArray propertiesArray) continue;

                Property[] properties = new Property[propertiesArray.Count];
                for (int i = 0; i < propertiesArray.Count; i++)
                {
                    if (propertiesArray[i] is not JsonArray propertyArray) continue;

                    Property? property = ReadProperty(propertyArray);
                    if (property is null) continue;

                    properties[i] = property.Value;
                }

                output.Add(blockState.Value, data.Value, new Block(name, properties));
            }

            return output;
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
