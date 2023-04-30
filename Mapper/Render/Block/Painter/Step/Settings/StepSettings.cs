namespace Mapper
{
    public struct StepSettings
    {
        public StepCornerSettings ZNegCorner { get; set; }
        public StepCornerSettings XPosCorner { get; set; }
        public StepCornerSettings ZPosCorner { get; set; }
        public StepCornerSettings XNegCorner { get; set; }

        public Limit BelowTotalLimit { get; set; }
        public Limit AboveTotalLimit { get; set; }

        public float Increment { get; set; }

        public StepSettings()
        {
            ZNegCorner = StepCornerSettings.ZNegCorner;
            XPosCorner = StepCornerSettings.XPosCorner;
            ZPosCorner = StepCornerSettings.ZPosCorner;
            XNegCorner = StepCornerSettings.XNegCorner;

            BelowTotalLimit = new Limit(10, 10);
            AboveTotalLimit = new Limit(0, 1 / 4F);

            Increment = 1 / 6F;
        }
    }
}
