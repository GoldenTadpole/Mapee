using System.Text.Json.Nodes;

namespace WorldEditor
{
    public class BiomeColorReader : IObjectReader<BiomeColorReadArgs, IBiomeColor?>
    {
        public virtual IBiomeColor? Read(BiomeColorReadArgs input)
        {
            JsonObject? effects = input.ParentToken["effects"]?.AsObject();
            if (effects is null) return null;

            if (!effects.TryGetPropertyValue(input.TokenName, out JsonNode? token)) 
            {
                return ReadColormapColor(input.ParentToken);
            } 
            
            if (effects.TryGetPropertyValue("grass_color_modifier", out JsonNode? modifier) && modifier?.GetValue<string>() != "none")
            {
                return new ModifiedBiomeColor()
                {
                    Modifier = modifier?.GetValue<string>()
                };
            }

            return new HardCodedBiomeColor()
            {
                Color = token?.GetValue<int>() ?? 0
            };
        }
        protected virtual ColormappedBiomeColor ReadColormapColor(JsonObject obj)
        {
            return new ColormappedBiomeColor()
            {
                Temperature = obj["temperature"]?.GetValue<float>() ?? 0,
                Downfall = obj["downfall"]?.GetValue<float>() ?? 0
            };
        }
    }
}
