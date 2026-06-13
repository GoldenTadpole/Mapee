namespace WorldEditor
{
    public interface IBlockRenamer
    {
        bool Rename(Block block, Version to, out Block output);
    }
}
