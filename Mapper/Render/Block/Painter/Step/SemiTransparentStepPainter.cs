namespace Mapper
{
    public class SemiTransparentStepPainter : StepPainter
    {
        public virtual float Multiplier { get; set; } = 1.5F;

        protected override VecRgb CalculateStep(StepPainterArgs input)
        {
            VecRgb step = base.CalculateStep(input);
            return step + (step - 1) * Multiplier;
        }
    }
}
