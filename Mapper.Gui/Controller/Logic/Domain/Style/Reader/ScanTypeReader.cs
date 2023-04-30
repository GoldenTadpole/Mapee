using System.IO;
using System.Text.Json.Nodes;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class ScanTypeReader : IObjectReader<StyleSettingsArgs>
    {
        public void Read(StyleSettingsArgs args)
        {
            byte[]? jsonBytes = args.Input.ReadFile("Settings\\Scan.json");
            if (jsonBytes is null) return;

            JsonObject? obj = JsonNode.Parse(ReadToEnd(jsonBytes))?.AsObject();
            if (obj is null) return;

            string? scanType = obj["ScanType"]?.AsValue().GetValue<string>();
            if (!string.IsNullOrEmpty(scanType) && scanType == "Cave")
            {
                args.Output.ScanType = ScanType.Cave;
            }
            else 
            {
                args.Output.ScanType = ScanType.Default;
            }
        }

        private static string ReadToEnd(byte[] bytes)
        {
            using MemoryStream input = new(bytes);
            using StreamReader reader = new(input);
            return reader.ReadToEnd();
        }
    }
}
