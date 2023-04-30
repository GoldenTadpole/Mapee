namespace WorldEditor
{
    public class CustomBiome
    {
        public string Namespace { get; set; }
        public string Dimension { get; set; }

        public IBiomeColor? GrassColor { get; set; }
        public IBiomeColor? FolliageColor { get; set; }
        public IBiomeColor? WaterColor { get; set; }

        public CustomBiome(string @namespace) 
        {
            Namespace = @namespace;
            Dimension = string.Empty;
        }
    }
}
