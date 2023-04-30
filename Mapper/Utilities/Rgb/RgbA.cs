namespace Mapper
{
    public readonly struct RgbA
    {
        public VecRgb Rgb { get; init; }
        public float A { get; init; }

        public static RgbA Empty => new RgbA(VecRgb.Empty);

        public RgbA()
        { 
            Rgb = VecRgb.Empty;
            A = 1;
        }
        public RgbA(VecRgb rgb)
        {
            Rgb = rgb;
            A = 1;
        }
        public RgbA(VecRgb rgb, float a)
        {
            Rgb = rgb;
            A = a;
        }

        public RgbA SetOpacity(float a)
        {
            return new RgbA(Rgb, a);
        }
        public bool IsEmpty()
        {
            return Rgb.IsEmpty();
        }
    }
}
