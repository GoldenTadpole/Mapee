namespace WorldEditor
{
    public class IdTranslator : IIdTranslator
    {
        protected IDictionary<int, Block> IDs { get; set; }

        public IdTranslator()
        {
            IDs = new Dictionary<int, Block>();
        }

        public static IdTranslator FromFile(string file)
        {
            return new IdTranslatorReader().Read(File.ReadAllText(file));
        }

        public virtual Block Translate(byte blockState, byte data)
        {
            if (!IDs.TryGetValue(ChunkUtilities.CalculateHashCode(blockState, data), out Block block))
            {
                block = TryFindBlock(blockState, data);
                if (block.IsEmpty()) return default;
            }

            Property[] properties = new Property[block.Properties.Length];
            block.Properties.CopyTo(properties, 0);

            return new Block(block.Name, properties);
        }
        protected Block TryFindBlock(byte blockState, byte data, int countLeft = 16)
        {
            if (countLeft == 0) return default;

            if (!IDs.TryGetValue(ChunkUtilities.CalculateHashCode(blockState, data), out Block block))
            {
                return TryFindBlock(blockState, (byte)((data + 1) % 16), countLeft--);
            }

            return block;
        }

        public void Add(byte blockState, byte data, Block block)
        {
            IDs.Add(ChunkUtilities.CalculateHashCode(blockState, data), block);
        }
    }
}
