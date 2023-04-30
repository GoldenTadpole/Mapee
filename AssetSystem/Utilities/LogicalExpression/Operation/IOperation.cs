namespace AssetSystem
{
    public interface IOperation
    {
        bool Execute(CallStack stack, PropertyValueProvider propertyValueProvider);
    }
}
