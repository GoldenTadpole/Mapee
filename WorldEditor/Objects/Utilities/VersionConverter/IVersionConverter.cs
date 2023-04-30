namespace WorldEditor
{
    public interface IVersionConverter
    {
        IObject? Convert(IObject input, Version from, Version to, UsageIntent intent);
    }
}
