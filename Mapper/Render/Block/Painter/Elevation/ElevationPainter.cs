namespace Mapper
{
    public class ElevationPainter : IElevationPainter
    {
        public float YOffset { get; set; } = 0;

        public virtual VecRgb Paint(VecRgb baseColor, VecRgb intensity, int y, ElevationSettings settings)
        {
            VecRgb elevation = CalculateElevation(y, settings) * intensity;

            if (!settings.MultiplyMaxIncrement || elevation.GetBrightness() < 0)
            {
                return (baseColor + elevation).Clamp();
            }

            return (baseColor * (settings.Hue + elevation)).Clamp();
        }
        protected virtual VecRgb CalculateElevation(int firstInstanceOfY, ElevationSettings settings)
        {
            float y = firstInstanceOfY - settings.Y + YOffset;
            y = y < settings.MinSteps ? settings.MinSteps : y > settings.MaxSteps ? settings.MaxSteps : y;

            y *= y > 0 ? settings.MaxIncrement : settings.MinIncrement;

            return y;
        }
    }
}
