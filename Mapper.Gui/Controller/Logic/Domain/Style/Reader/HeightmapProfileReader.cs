using Mapper.Gui.Model;
using System;
using System.IO;
using System.Text.Json.Nodes;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class HeightmapProfileReader : IObjectReader<StyleSettingsArgs>
    {
        public void Read(StyleSettingsArgs args)
        {
            byte[]? jsonBytes = args.Input.ReadFile("Settings\\Heightmap.json");
            if (jsonBytes is null) return;

            JsonObject? obj = JsonNode.Parse(ReadToEnd(jsonBytes))?.AsObject();
            if (obj is null) return;

            JsonObject? defaultNode = obj["Default"]?.AsObject();
            if (defaultNode is null) return;

            HeightmapSettings? defaultProfile = ReadProfile(defaultNode);
            if (defaultProfile is null) return;
            args.Output.DefaultHeightmapStyle = defaultProfile.Value;

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

                JsonObject? profileNode = dimensionObj["Settings"]?.AsObject();
                if (profileNode is null) return;

                HeightmapSettings? profile = ReadProfile(profileNode);
                if (profile is null) continue;

                args.Output.DimensionHeightmapStyles?.Add(dimension, profile.Value);
            }
        }

        private static string ReadToEnd(byte[] bytes)
        {
            using MemoryStream input = new(bytes);
            using StreamReader reader = new(input);
            return reader.ReadToEnd();
        }
        private static HeightmapSettings? ReadProfile(JsonObject obj) 
        {
            string? type = null;
            if (obj.TryGetPropertyValue("Type", out JsonNode? typeNode) && typeNode is not null)
            {
                type = typeNode?.AsValue().GetValue<string>();
            }
            if (type is null) return null;

            short? setY = null;
            if (obj.TryGetPropertyValue("SetY", out JsonNode? setYNode) && setYNode is not null)
            {
                setY = setYNode?.AsValue().GetValue<short>();
            }
            if (setY is null) return null;

            string? heightmap = null;
            if (obj.TryGetPropertyValue("Heightmap", out JsonNode? heightmapNode) && heightmapNode is not null)
            {
                heightmap = heightmapNode?.AsValue().GetValue<string>();
            }
            if (heightmap is null) return null;

            return new HeightmapSettings() 
            {
                HeightmapType = type == "SetY" ? HeightmapType.SetY : HeightmapType.NbtHeightmap,
                SetY = setY.Value,
                NbtHeightmap = heightmap
            };
        }
    }
}
