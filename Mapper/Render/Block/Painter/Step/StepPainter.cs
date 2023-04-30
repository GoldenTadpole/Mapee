namespace Mapper
{
    public class StepPainter : IStepPainter
    {
        public VecRgb Paint(VecRgb baseColor, StepPainterArgs input)
        {
            return (baseColor * CalculateStep(input)).Clamp();
        }

        protected virtual VecRgb CalculateStep(StepPainterArgs input)
        {
            StepSettings settings = input.BlockPainterParameter.Controller.GetStepSettings(input.BlockPainterParameter.Parameter.Block);

            float stepY = 0;

            stepY += AddStep(input.Step.ZNeg, settings.ZNegCorner);
            stepY += AddStep(input.Step.XPos, settings.XPosCorner);
            stepY += AddStep(input.Step.ZPos, settings.ZPosCorner);
            stepY += AddStep(input.Step.XNeg, settings.XNegCorner);

            if (stepY < -settings.BelowTotalLimit.Max) stepY = -settings.BelowTotalLimit.MaxReturnedValue;
            else if (stepY > settings.AboveTotalLimit.Max) stepY = settings.AboveTotalLimit.MaxReturnedValue;

            return 1 + stepY * settings.Increment;
        }
        protected virtual float AddStep(float step, StepCornerSettings corner)
        {
            if (step < 0) return -CheckLimit(-step, corner.BelowLimit) * corner.BelowIncrement;
            return CheckLimit(step, corner.AboveLimit) * corner.AboveIncrement;
        }
        private static float CheckLimit(float step, Limit limit)
        {
            if (step <= limit.Min) return limit.MinReturnedValue;
            else if (step <= limit.Max) return step;

            return limit.MaxReturnedValue;
        }
    }
}
