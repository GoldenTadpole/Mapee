namespace AssetSystem.Biome
{
    public readonly struct BiomeBlock
    {
        public WorldEditor.Block Block { get; init; }
        public string Biome { get; init; }

        public BiomeBlock(WorldEditor.Block block, string biome) 
        {
            Block = block;
            Biome = biome;
        }

        public override int GetHashCode()
        {
            return GetHashCode(Block.Name.GetHashCode(), Biome.GetHashCode());
        }
        public static int GetHashCode(int blockHash, int biomeHash)
        {
            return HashCode.Combine(blockHash, biomeHash);
        }
    }
}
