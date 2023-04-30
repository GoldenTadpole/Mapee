using MapScanner;

namespace Mapper
{
    public class LightPainter : ILightPainter
    {
        public virtual float AmbientLight { get; set; } = 0.125F;
        public virtual float SunIntensity { get; set; } = 1;

        public virtual VecRgb FindLight(BlockData blockData)
        {
            Light light = new Light(blockData.SkyLight, blockData.BlockLight);

            float total = Math.Min((light.SkyLight * SunIntensity) + light.BlockLight, 15) / 15;
            total *= 1 - AmbientLight;
            total += AmbientLight;

            return Math.Max(0, Math.Min(1, total));
        }
        public virtual VecRgb PaintBlock(VecRgb baseColor, VecRgb element)
        {
            return (baseColor * element).Clamp();
        }
    }
}
