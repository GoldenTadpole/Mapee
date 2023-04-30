namespace AssetSystem
{
    public class Expression
    {
        public IList<IOperation> Operations { get; set; }
        public int Level { get; set; } = 0;

        public Expression(int level)
        {
            Operations = new List<IOperation>();
            Level = level;
        }
        public Expression(IList<IOperation> operations, int level)
        {
            Operations = operations;
            Level = level;
        }
        public Expression(IList<Expression> expressions, int level)
        {
            List<IOperation> operations = new List<IOperation>();

            foreach (Expression expression in expressions)
            {
                operations.AddRange(expression.Operations);
            }

            Operations = operations;
            Level = level;
        }
    }
}
