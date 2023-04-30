namespace AssetSystem
{
    public readonly struct PropertyMatcher<TOutput> : ICloneable
    {
        public TOutput Payload { get; }
        public LogicalExpression? Expression { get; }

        public PropertyMatcher(TOutput payload, LogicalExpression? expression)
        {
            Payload = payload;
            Expression = expression;
        }

        public bool Match(PropertyValueProvider propertyValueProvider) 
        {
            if (Expression is null) return true;
            return Expression.Run(propertyValueProvider);
        }

        public object Clone()
        {
            return new PropertyMatcher<TOutput>(Payload, (LogicalExpression?) Expression?.Clone() ?? null);
        }
    }
}
