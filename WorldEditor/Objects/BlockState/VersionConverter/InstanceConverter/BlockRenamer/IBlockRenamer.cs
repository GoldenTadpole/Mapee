namespace WorldEditor
{
    public interface IBlockRenamer
    {
        bool Rename(Block block, out Block output);
    }
}
