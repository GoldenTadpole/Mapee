namespace Mapper
{
    public struct Colormap
    {
        public IReadOnlyBitmap Grass { get; set; }
        public IReadOnlyBitmap Foliage { get; set; }

        public Colormap(IReadOnlyBitmap grass, IReadOnlyBitmap foliage) 
        {
            Grass = grass;
            Foliage = foliage;
        }
    }
}
