namespace WorldEditor
{
    public class BlockTimeline
    {
        public IList<(Version Version, Block Block)> Blocks { get; }

        public BlockTimeline()
        {
            Blocks = new List<(Version Version, Block Block)>();
        }

        public bool Find(Version targetVersion, out Block output)
        {
            for (int i = 0; i < Blocks.Count; i++)
            {
                if (targetVersion >= Blocks[i].Version) continue;

                output = i > 0 ? Blocks[i - 1].Block : Blocks[0].Block;
                return true;
            }

            if (Blocks.Count > 0)
            {
                output = Blocks[Blocks.Count - 1].Block;
                return true;
            }

            output = default;
            return false;
        }
    }
}
