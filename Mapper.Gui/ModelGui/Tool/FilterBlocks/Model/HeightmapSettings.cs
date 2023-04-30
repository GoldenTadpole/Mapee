namespace Mapper.Gui.Model
{
    public struct HeightmapSettings
    {
        public HeightmapType HeightmapType { get; set; }
        public short SetY { get; set; }
        public string NbtHeightmap { get; set; }

        public HeightmapSettings()
        {
            HeightmapType = HeightmapType.NbtHeightmap;
            SetY = 319;
            NbtHeightmap = "WORLD_SURFACE";
        }
    }
}
