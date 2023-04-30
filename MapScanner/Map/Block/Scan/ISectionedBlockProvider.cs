using System;
using WorldEditor;

namespace MapScanner
{
    public interface ISectionedBlockProvider : IDisposable
    {
        BlockGrouping ProvideGrouping(int index);
        Block ProvideBlock(int index);
        short ProvideIndexInPalette(int index);
        BlockData ProvideBlockData(int index, bool isTopBlock = false);
    }
}
