namespace AssetSystem
{
    public class ComparisonOperation : IOperation
    {
        public ComparisonOperand Operand { get; set; }

        public string PropertyName { get; set; }
        public string[] PropertyValues { get; set; }

        public bool MustIncludeAllProperties { get; set; }

        public ComparisonOperation(string propertyName, string[] propertyValues)
        {
            PropertyName = propertyName;
            PropertyValues = propertyValues;
        }

        public bool Execute(CallStack stack, PropertyValueProvider propertyValueProvider)
        {
            stack.Advance();

            string value = propertyValueProvider(PropertyName);
            if (value == null) return !MustIncludeAllProperties;

            switch (Operand)
            {
                case ComparisonOperand.Equals:
                    return PropertyValues[0] == value;
                case ComparisonOperand.NotEquals:
                    return PropertyValues[0] != value;
                case ComparisonOperand.ArrayAndNotEquals:
                    for (int i = 0; i < PropertyValues.Length; i++)
                    {
                        if (PropertyValues[i] == value) return false;
                    }

                    return true;
                case ComparisonOperand.ArrayOrEquals:
                    for (int i = 0; i < PropertyValues.Length; i++)
                    {
                        if (PropertyValues[i] == value) return true;
                    }

                    return false;
            }

            return false;
        }
    }
}
