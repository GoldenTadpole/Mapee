using WorldEditor;

namespace MapScanner
{
    public interface IBlockOutput
    {
        bool GiveBlock(ScannedBlock block, Block representedBlock);
        bool GiveBlockSpan(BlockSpan span, Block representedBlock, bool isMaxReached = false);

        void BeginScan();
        void EndScan();
    }
}
