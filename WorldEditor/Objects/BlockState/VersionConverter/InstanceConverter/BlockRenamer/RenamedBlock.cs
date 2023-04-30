namespace WorldEditor
{
    public class RenamedBlock
    {
        public string OldNamespace { get; set; }

        public IList<Block> OldBlocks { get; set; }
        public IList<Block> NewBlocks { get; set; }

        public RenamedBlock(string oldNamespace)
        {
            OldNamespace = oldNamespace;
            OldBlocks = new List<Block>();
            NewBlocks = new List<Block>();
        }

        public Block Rename(Block oldBlock)
        {
            if (oldBlock.Name != OldNamespace) return default;

            for (int i = 0; i < OldBlocks.Count; i++)
            {
                if (PropertiesEquals(oldBlock.Properties, OldBlocks[i].Properties))
                {
                    return CloneBlock(NewBlocks[i]);
                }
            }

            return CloneBlock(NewBlocks[0]);
        }

        private static bool PropertiesEquals(Property[] left, Property[] right)
        {
            if (left.Length != right.Length) return false;

            for (int i = 0; i < left.Length; i++)
            {
                if (left[i].Name != right[i].Name ||
                    left[i].Value != right[i].Value) return false;
            }

            return true;
        }
        private static Block CloneBlock(Block block)
        {
            Property[] properties = new Property[block.Properties.Length];
            Array.Copy(block.Properties, properties, properties.Length);

            return new Block(block.Name, properties);
        }
    }
}
