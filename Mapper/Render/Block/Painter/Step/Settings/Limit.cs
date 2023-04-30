namespace Mapper
{
    public readonly struct Limit
    {
        public short Max { get; init; }
        public float MaxReturnedValue { get; init; }

        public short Min { get; init; }
        public float MinReturnedValue { get; init; }

        public Limit()
        {
            Max = 0;
            MaxReturnedValue = 0;
            Min = 0;
            MinReturnedValue = 0;
        }
        public Limit(short max, float maxReturnedValue)
        {
            Max = max;
            MaxReturnedValue = maxReturnedValue;
            Min = 0;
            MinReturnedValue = 0;
        }
        public Limit(short max, float maxReturnedValue, short min, float minReturnedValue)
        {
            Max = max;
            MaxReturnedValue = maxReturnedValue;
            Min = min;
            MinReturnedValue = minReturnedValue;
        }
    }
}
