using Mapper.Gui.Model;
using System.IO;
using System.Text.Json.Nodes;
using System.Windows.Media;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class RenderSettingsReader : IObjectReader<StyleSettingsArgs>
    {
        public void Read(StyleSettingsArgs args)
        {
            byte[]? jsonBytes = args.Input.ReadFile("Settings\\Render.json");
            if (jsonBytes is null) return;

            JsonObject? obj = JsonNode.Parse(ReadToEnd(jsonBytes))?.AsObject();
            if (obj is null) return;

            JsonObject? defaultNode = obj["Default"]?.AsObject();
            if (defaultNode is null) return;

            RenderSettings? defaultSettings = ReadSettings(defaultNode);
            if (defaultSettings is null) return;
            args.Output.DefaultRenderSettingStyle = defaultSettings.Value;

            JsonArray? dimensionArray = obj["Dimensions"]?.AsArray();
            if (dimensionArray is null) return;

            foreach (JsonNode? dimensionNode in dimensionArray)
            {
                if (dimensionNode is null || dimensionNode is not JsonObject dimensionObj) continue;

                string? dimension = null;
                if (dimensionObj.TryGetPropertyValue("Dimension", out JsonNode? dimensionTypeNode) && dimensionTypeNode is not null)
                {
                    dimension = dimensionTypeNode?.AsValue().GetValue<string>();
                }
                if (dimension is null) continue;

                JsonObject? settingsNode = dimensionObj["Settings"]?.AsObject();
                if (settingsNode is null) return;

                RenderSettings? profile = ReadSettings(settingsNode);
                if (profile is null) continue;

                args.Output.DimensionRenderSettingStyles?.Add(dimension, profile.Value);
            }
        }

        private static string ReadToEnd(byte[] bytes)
        {
            using MemoryStream input = new(bytes);
            using StreamReader reader = new(input);
            return reader.ReadToEnd();
        }
        private static RenderSettings? ReadSettings(JsonObject obj)
        {
            float? skylight = ReadValue(obj, "SkyLight");
            if (skylight is null) return null;

            float? ambientLight = ReadValue(obj, "AmbientLight");
            if (ambientLight is null) return null;

            float? altitudeYOffset = ReadValue(obj, "AltitudeYOffset");
            if (altitudeYOffset is null) return null;

            float? transparentBlockStepIntensity = ReadValue(obj, "TransparentBlockStepIntensity");
            if (transparentBlockStepIntensity is null) return null;

            Background? background = ReadBackground(obj["Background"]?.AsObject());
            if (background is null) return null;

            return new RenderSettings()
            {
                SkyLightIntensity = skylight.Value,
                AmbientLightIntensity = ambientLight.Value,
                AltitudeYOffset = altitudeYOffset.Value,
                SemiTransparentStepIntensity = transparentBlockStepIntensity.Value,
                Background = background.Value
            };
        }

        private static float? ReadValue(JsonObject obj, string name) 
        {
            float? output = null;
            if (obj.TryGetPropertyValue(name, out JsonNode? node) && node is not null)
            {
                output = node?.AsValue().GetValue<float>();
            }

            return output;
        }

        private static Background? ReadBackground(JsonObject? obj) 
        {
            if (obj is null) return null;

            bool? checker = null;
            if (obj.TryGetPropertyValue("IsChecker", out JsonNode? node) && node is not null)
            {
                checker = node?.AsValue().GetValue<bool>();
            }

            if (checker is null) return null;

            ColorPair colorPair = ReadColorPair(obj);
            Color solidColor = ReadSolidColor(obj);

            return new Background() 
            {
                Type = checker.Value ? BackgroundType.Checker : BackgroundType.Solid,
                CheckedColorPair = colorPair,
                SolidColor = solidColor
            };
        }

        private static ColorPair ReadColorPair(JsonObject obj) 
        {
            JsonArray? pairArray = obj["ColorPair"]?.AsArray();
            if (pairArray is not null)
            {
                Color even = ReadColor(pairArray[0]?.AsArray()) ?? Colors.Black;
                Color odd = ReadColor(pairArray[1]?.AsArray()) ?? Colors.Black;

                return new ColorPair(even, odd);
            }

            return new ColorPair();
        }
        private static Color ReadSolidColor(JsonObject obj) 
        {
            return ReadColor(obj["SolidColor"]?.AsArray()) ?? Colors.Black;
        }
        private static Color? ReadColor(JsonArray? colorArray) 
        {
            if (colorArray is null || colorArray.Count < 3) return null;

            byte? r = colorArray[0]?.AsValue().GetValue<byte>();
            byte? g = colorArray[1]?.AsValue().GetValue<byte>();
            byte? b = colorArray[2]?.AsValue().GetValue<byte>();

            if (r is null || g is null || b is null) return null;

            return Color.FromRgb(r.Value, g.Value, b.Value);
        }
    }
}
