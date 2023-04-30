namespace WorldEditor
{
    public class LightVersionConverter : VersionConverter
    {
        protected override void InitializeConverters()
        {
            Converters.Add(new LightInstanceConverter());
        }
    }
}
