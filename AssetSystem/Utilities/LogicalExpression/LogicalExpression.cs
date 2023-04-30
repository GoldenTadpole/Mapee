namespace AssetSystem
{
    public class LogicalExpression : ICloneable
    {
        public string Expression { get; private set; }
        public bool MustIncludeAllProperties { get; private set; }

        private readonly IOperation[] _operations;

        public LogicalExpression(string expression, bool mustIncludeAllProperties = true)
        {
            Expression = expression;
            MustIncludeAllProperties = mustIncludeAllProperties;

            _operations = new OperationStackReader(MustIncludeAllProperties).Read(Expression);
        }

        public bool Run(PropertyValueProvider propertyValueProvider)
        {
            return new CallStack(_operations).Run(propertyValueProvider);
        }

        public object Clone()
        {
            return new LogicalExpression(Expression, MustIncludeAllProperties);
        }
    }
}
