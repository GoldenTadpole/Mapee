using CommonUtilities.Collections.Simple;
using System.Collections.Generic;
using WorldEditor;

namespace MapScanner
{
    public class ScannedChunk : IScannedChunk
    {
        public Coords Coords { get; set; }

        public IList<ScannedColumn> UniqueColumns { get; set; }
        public byte[] Indexes { get; set; }

        public Section<Block>[]? BlockSections { get; set; }
        public Section<string>[]? BiomeSections { get; set; }

        public ScannedChunk(Coords coords)
        {
            Coords = coords;

            UniqueColumns = new SimpleList<ScannedColumn>(256);
            Indexes = new byte[256];
        }

        public ScannedColumn GetColumn(int index)
        {
            return UniqueColumns[Indexes[index]];
        }
    }
}
