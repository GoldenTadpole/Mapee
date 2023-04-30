namespace Mapper.Gui.Model
{
    public struct RenderSettings
    {
        public float SkyLightIntensity { get; set; } = 1;
        public float AmbientLightIntensity { get; set; } = 0;

        public float AltitudeYOffset { get; set; } = 0;
        public float SemiTransparentStepIntensity { get; set; } = 2;

        public Background Background { get; set; }

        public RenderSettings() { }
    }
}
