using CommonUtilities.Factory;
using System;
using System.Collections.Generic;
using WorldEditor;

namespace MapScanner
{
    public class ChunkScanner : IObjectScanner<ConvertedApiChunk, ScannedChunk>
    {
        public IObjectScanner<ColumnScanArgs> ColumnScanner { get; set; }
        public IFactory<ColumnScanArgsFactoryArgs, ColumnScanArgs> ColumnScanParameterFactory { get; set; }

        public ChunkScanner(IObjectScanner<ColumnScanArgs> columnScanner, IFactory<ColumnScanArgsFactoryArgs, ColumnScanArgs> columnScanParameterFactory)
        {
            ColumnScanner = columnScanner;
            ColumnScanParameterFactory = columnScanParameterFactory;
        }

        public ScannedChunk Scan(ConvertedApiChunk chunk)
        {
            ScannedChunk output = new(new Coords(chunk.X, chunk.Z));
            ColumnScanArgs scanParameter = ColumnScanParameterFactory.Create(new ColumnScanArgsFactoryArgs() 
            {
                ApiChunk = chunk,
                ScannedChunk = output
            });

            int maxY = int.MinValue, minY = int.MaxValue;
            for (int i = 0; i < 256; i++)
            {
                scanParameter.BlockOutput.BeginScan();
                ColumnScanner.Scan(scanParameter.Copy(i % 16, i / 16));
                scanParameter.BlockOutput.EndScan();

                ScannedColumn column = output.GetColumn(i);
                if (column.Type == ColumnType.Empty) continue;

                GetColumnRange(column, out int colMaxY, out int colMinY);
                if (colMaxY > maxY) maxY = colMaxY;
                if (colMinY < minY) minY = colMinY;
            }

            SetSections(maxY, minY, chunk, output);
            scanParameter.SectionCollection.Dispose();

            return output;
        }

        private static void GetColumnRange(ScannedColumn column, out int maxY, out int minY)
        {
            if (column.BlockSpans.Length > 0)
            {
                ReadOnlySpan<BlockSpan> span = column.BlockSpans.Span;
                maxY = span[0].TopY;

                if (column.BottomBlock.IsEmpty())
                {
                    minY = span[column.BlockSpans.Length - 1].EndY;
                }
                else
                {
                    minY = column.BottomBlock.FirstInstanceY;
                }
            }
            else
            {
                maxY = column.BottomBlock.FirstInstanceY;
                minY = maxY;
            }
        }

        private static void SetSections(int maxY, int minY, ConvertedApiChunk chunk, ScannedChunk output)
        {
            if (maxY == int.MinValue && minY == int.MaxValue)
            {
                output.BlockSections = Array.Empty<Section<Block>>();
                output.BiomeSections = Array.Empty<Section<string>>();

                return;
            }

            maxY = MathUtilities.FindSectionY(maxY);
            minY = MathUtilities.FindSectionY(minY);

            int count = maxY - minY + 1;

            Section<Block>[] blockSections = new Section<Block>[count];
            Section<string>[] biomeSections = new Section<string>[count];

            for (int y = 0; y < count; y++)
            {
                sbyte yIndex = (sbyte)(y + minY);

                if (chunk.BlockState is not null && FindSection(yIndex, chunk.BlockState.Sections, out ISection? blockStateSection) && blockStateSection is not null)
                {
                    blockSections[y] = new Section<Block>(yIndex, ((PaletteSection<Block>)blockStateSection).Palette);
                }

                if (chunk.Biome is not null && FindSection(yIndex, chunk.Biome.Sections, out ISection? biomeSection) && biomeSection is not null)
                {
                    biomeSections[y] = new Section<string>(yIndex, ((PaletteSection<string>)biomeSection).Palette);
                }
            }

            output.BlockSections = blockSections;
            output.BiomeSections = biomeSections;
        }
        private static bool FindSection(sbyte y, IEnumerable<ISection> sections, out ISection? section)
        {
            foreach (ISection s in sections)
            {
                if (s.Y != y) continue;

                section = s;
                return true;
            }

            section = null;
            return false;
        }
    }
}
