namespace WorldEditor
{
    public class BlockRenamer : IBlockRenamer
    {
        public IDictionary<string, RenamedBlock> Blocks { get; set; }

        public BlockRenamer()
        {
            Blocks = new Dictionary<string, RenamedBlock>();
        }

        public bool Rename(Block block, out Block output)
        {
            output = default;
            if (!Blocks.TryGetValue(block.Name, out RenamedBlock? renamedBlock)) return false;

            output = renamedBlock.Rename(block);
            return true;
        }

        public static BlockRenamer FromFile(string file)
        {
            return new BlockRenamerReader().Read(File.ReadAllText(file));
        }
    }
}
