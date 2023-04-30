namespace Mapper
{
    public readonly struct DepthOpacity : IEquatable<DepthOpacity>
    {
        public float Opacity { get; init; }
        public OpacityType OpacityType { get; init; }
        public float MaxDepth { get; init; }

        private readonly int _identifier;

        public DepthOpacity()
        {
            Opacity = 1;
            OpacityType = OpacityType.Multiply;
            MaxDepth = 1;
            _identifier = int.MinValue;
        }
        public DepthOpacity(float opacity, OpacityType opacityType, float maxDepth)
        {
            Opacity = opacity;
            OpacityType = opacityType;
            MaxDepth = maxDepth;
            _identifier = int.MinValue;
        }
        public DepthOpacity(float opacity, OpacityType opacityType, float maxDepth, int identifier)
        {
            Opacity = opacity;
            OpacityType = opacityType;
            MaxDepth = maxDepth;
            _identifier = identifier;
        }

        public float CalculateOpacity(float opacity)
        {
            if (OpacityType == OpacityType.DecraseTransparency) return 1 - (1 - opacity) / Opacity;
            else if (OpacityType == OpacityType.Multiply) return Opacity * opacity;
            return Opacity;
        }
        public bool Equals(DepthOpacity other)
        {
            if (_identifier == int.MinValue || other._identifier == int.MinValue) return false;

            return _identifier == other._identifier;
        }
    }
}