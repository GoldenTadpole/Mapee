namespace Mapper
{
    public interface IElevationPainter
    {
        VecRgb Paint(VecRgb baseColor, VecRgb intensity, int y, ElevationSettings elevationSettings);
    }
}
