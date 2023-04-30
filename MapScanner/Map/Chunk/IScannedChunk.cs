using System.Collections.Generic;
using WorldEditor;

namespace MapScanner
{
    public interface IScannedChunk
    {
        Coords Coords { get; }

        IList<ScannedColumn> UniqueColumns { get; }
        byte[] Indexes { get; }

        Section<Block>[]? BlockSections { get; set; }
        Section<string>[]? BiomeSections { get; set; }

        ScannedColumn GetColumn(int index);
    }
}
