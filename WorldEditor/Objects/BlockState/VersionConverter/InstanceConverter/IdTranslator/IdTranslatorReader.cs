using System.Text.Json.Nodes;

namespace WorldEditor
{
    public class IdTranslatorReader : IObjectReader<string, IdTranslator>
    {
        public IdTranslator Read(string jsonString)
        {
            IdTranslator output = new();

            JsonArray? jsonArray = JsonNode.Parse(jsonString)?.AsArray();
            if (jsonArray == null) return output;

            foreach (JsonNode? node in jsonArray)
            {
                if (node == null) continue;

                JsonObject? obj = node.AsObject();
                if (obj == null) continue;

                byte id = (byte)(obj["Id"]?.GetValue<int>() ?? 0);
                byte data = (byte)(obj["Data"]?.GetValue<int>() ?? 0);
                string blockString = obj["Block"]?.GetValue<string>() ?? string.Empty;

                Block block = ParseBlock(blockString);
                output.Add(id, data, block);
            }

            return output;
        }

        private Block ParseBlock(string blockString)
        {
            if (string.IsNullOrEmpty(blockString)) return Block.Empty;

            string[] parts = blockString.Split(' ', 2);
            string name = parts[0];

            if (parts.Length == 1)
            {
                return new Block(name);
            }

            string propertiesString = parts[1];
            string[] propertyPairs = propertiesString.Split(';');
            Property[] properties = new Property[propertyPairs.Length];

            for (int i = 0; i < propertyPairs.Length; i++)
            {
                string[] keyValue = propertyPairs[i].Split('=', 2);
                if (keyValue.Length == 2)
                {
                    properties[i] = new Property(keyValue[0], keyValue[1]);
                }
                else
                {
                    properties[i] = new Property(keyValue[0], string.Empty);
                }
            }

            return new Block(name, properties);
        }
    }
}
