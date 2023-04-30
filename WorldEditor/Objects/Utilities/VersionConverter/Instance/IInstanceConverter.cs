namespace WorldEditor
{
    public interface IInstanceConverter<out TOutput>
    {
        VersionRange From { get; }
        VersionRange To { get; }

        TOutput? Convert(IObject input, UsageIntent intent);
    }
}
