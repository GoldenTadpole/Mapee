using AssetSystem;
using CommonUtilities.Data;
using System.IO;
using System.Text.Json.Nodes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class StyleMetadataReader : IObjectReader<IDataReader, StyleMetadata?>
    {
        public StyleMetadata? Read(IDataReader data)
        {
            ImageSource? icon = ReadImage(data);
            if (icon is null) return null;

            StyleMetadata? output = ReadMetadata(data);
            if (output is null) return null;

            output.Icon = icon;

            return output;
        }

        private static ImageSource? ReadImage(IDataReader data) 
        {
            byte[]? iconBytes = data.ReadFile("Icon.png");
            if (iconBytes is null) return null;

            return ReadImage(iconBytes);
        }
        private static StyleMetadata? ReadMetadata(IDataReader data) 
        {
            byte[]? jsonBytes = data.ReadFile("Metadata.json");
            if(jsonBytes is null) return null;

            JsonObject? obj = JsonNode.Parse(ReadToEnd(jsonBytes))?.AsObject();
            if (obj is null) return null;

            string? name = null;
            if (obj.TryGetPropertyValue("Name", out JsonNode? nameNode) && nameNode is not null)
            {
                name = nameNode?.AsValue().GetValue<string>();
            }
            if (name is null) return null;

            string? id = null;
            if (obj.TryGetPropertyValue("Id", out JsonNode? idNode) && idNode is not null)
            {
                id = idNode?.AsValue().GetValue<string>();
            }
            if (id is null) return null;

            int? index = null;
            if (obj.TryGetPropertyValue("OrderedIndex", out JsonNode? indexNode) && indexNode is not null)
            {
                index = indexNode?.AsValue().GetValue<int>();
            }
            if (index is null) return null;

            string? allowedDimensions = null;
            if (obj.TryGetPropertyValue("AllowedDimensions", out JsonNode? allowedDimensionsNode) && allowedDimensionsNode is not null)
            {
                allowedDimensions = allowedDimensionsNode?.AsValue().GetValue<string>();
            }
            if(allowedDimensions is null) return null;

            bool? automaticallyDisableNight = false;
            if (obj.TryGetPropertyValue("AutomaticallyDisableNight", out JsonNode? automaticallyDisableNightNode) && automaticallyDisableNightNode is not null)
            {
                automaticallyDisableNight = automaticallyDisableNightNode?.AsValue().GetValue<bool>();
            }
            if (automaticallyDisableNight is null) return null;

            bool? disableDatapackStyle = false;
            if (obj.TryGetPropertyValue("DisableDatapackStyle", out JsonNode? disableDatapackStyleNode) && disableDatapackStyleNode is not null)
            {
                disableDatapackStyle = disableDatapackStyleNode?.AsValue().GetValue<bool>();
            }
            if (disableDatapackStyle is null) return null;

            return new StyleMetadata(name, id, index.Value, new LogicalExpression(allowedDimensions)) 
            {
                AutomaticallyDisableNight = automaticallyDisableNight.Value,
                DisableDatapackStyle = disableDatapackStyle.Value
            };
        }

        private static ImageSource ReadImage(byte[] bytes)
        {
            using MemoryStream input = new(bytes);

            BitmapImage output = new BitmapImage();
            output.BeginInit();
            output.StreamSource = input;
            output.CacheOption = BitmapCacheOption.OnLoad;
            output.EndInit();
            output.Freeze();

            return output;
        }
        private static string ReadToEnd(byte[] bytes) 
        {
            using MemoryStream input = new(bytes);
            using StreamReader reader = new(input);
            return reader.ReadToEnd();
        }
    }
}
