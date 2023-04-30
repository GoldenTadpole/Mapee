namespace Mapper
{
    public readonly struct ElevationSettings
    {
        public short Y { get; init; }

        public short MinSteps { get; init; }
        public float MinIncrement { get; init; }

        public short MaxSteps { get; init; }
        public float MaxIncrement { get; init; }

        public bool MultiplyMaxIncrement { get; init; }
        public VecRgb Hue { get; init; }

        public ElevationSettings()
        {
            Y = 70;
            MinSteps = -10;
            MinIncrement = 0.0156F;
            MaxSteps = 25;
            MaxIncrement = 0.0117F * 1.5F;
            MultiplyMaxIncrement = true;
            Hue = 1;
        }
    }
}
