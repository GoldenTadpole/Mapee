namespace WorldEditor
{
    public static class RegionStoreFactory
    {
        public static IRegionStore CreateRegionStore(Version version)
        {
            if (version < Version.Post_Beta_1_3)
            {
                return new AlphaRegionStore();
            }

            return new AnvilRegionStore();
        }
    }
}
