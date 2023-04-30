namespace AssetSystem
{
    public class LogicalOperation : IOperation
    {
        public LogicalOperand Operand { get; set; }

        public bool Execute(CallStack stack, PropertyValueProvider propertyValueProvider)
        {
            stack.Advance();

            switch (Operand)
            {
                case LogicalOperand.Negate:
                    return !stack.Next().Execute(stack, propertyValueProvider);
                case LogicalOperand.And:
                    if (!stack.Next().Execute(stack, propertyValueProvider))
                    {
                        stack.Advance();
                        return false;
                    }

                    return stack.Next().Execute(stack, propertyValueProvider);
                case LogicalOperand.Or:
                    if (stack.Next().Execute(stack, propertyValueProvider))
                    {
                        stack.Advance();
                        return true;
                    }

                    return stack.Next().Execute(stack, propertyValueProvider);
                case LogicalOperand.Xor:
                    return stack.Next().Execute(stack, propertyValueProvider) ^ stack.Next().Execute(stack, propertyValueProvider);
            }

            return false;
        }
    }
}
