namespace Mapper
{
    public interface IStepPainter
    {
        VecRgb Paint(VecRgb baseColor, StepPainterArgs input);
    }
}
