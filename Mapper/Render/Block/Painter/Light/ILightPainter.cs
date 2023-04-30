using MapScanner;

namespace Mapper
{
    public interface ILightPainter
    {
        VecRgb FindLight(BlockData blockData);
        VecRgb PaintBlock(VecRgb baseColor, VecRgb element);
    }
}
