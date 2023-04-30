namespace AssetSystem
{
    public static class PropertyValueProviderUtilities
    {
        public static PropertyValueProvider CreateGetter(WorldEditor.Property[] properties)
        {
            return name =>
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    if (properties[i].Name == name) return properties[i].Value;
                }

                return string.Empty;
            };
        }
    }
}
