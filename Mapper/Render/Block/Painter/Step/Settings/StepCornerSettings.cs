namespace Mapper
{
    public readonly struct StepCornerSettings
    {
        public Limit BelowLimit { get; init; }
        public float BelowIncrement { get; init; }

        public Limit AboveLimit { get; init; }
        public float AboveIncrement { get; init; }

        public static StepCornerSettings ZNegCorner { get; } = new StepCornerSettings()
        {
            BelowLimit = new Limit(short.MaxValue, 0),
            BelowIncrement = 1,
            AboveLimit = new Limit(1, 0),
            AboveIncrement = 1,
        };
        public static StepCornerSettings XPosCorner { get; set; } = new StepCornerSettings()
        {
            BelowLimit = new Limit(short.MaxValue, 0),
            BelowIncrement = 1,
            AboveLimit = new Limit(1, 0),
            AboveIncrement = 1,
        };
        public static StepCornerSettings ZPosCorner { get; set; } = new StepCornerSettings()
        {
            BelowLimit = new Limit(short.MaxValue, 0),
            BelowIncrement = 1 / 2.5F,
            AboveLimit = new Limit(1, 0),
            AboveIncrement = 1 / 2.5F,
        };
        public static StepCornerSettings XNegCorner { get; set; } = new StepCornerSettings()
        {
            BelowLimit = new Limit(short.MaxValue, 0),
            BelowIncrement = 1 / 5F,
            AboveLimit = new Limit(1, 0),
            AboveIncrement = 1 / 5F,
        };
    }
}
