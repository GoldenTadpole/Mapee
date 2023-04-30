using WorldEditor;

namespace MapScanner
{
    public class ColumnScanner : IObjectScanner<ColumnScanArgs>
    {
        public void Scan(ColumnScanArgs input)
        {
            if (input.BlockOutput is null) return;
            ProvideY(input, out int y, out int sectionY);

            bool isTop = true;

            BlockGrouping lastGrouping = BlockGrouping.Empty;
            Block lastBlock = Block.Empty;
            ScannedBlock currentScannedBlock = ScannedBlock.Empty;
            int currentCount = 0;
            bool checkBlock = false;

            while (sectionY >= input.SectionCollection.LowestSectionY)
            {
                ISectionedBlockProvider? provider = input.SectionCollection.Provide(sectionY--);
                if (provider is null)
                {
                    if (!currentScannedBlock.IsEmpty() && !AddCurrentBlockSpan(lastBlock)) return;
                    y = sectionY * 16 + 15;
                    checkBlock = false;
                    continue;
                }

                for (int index = MathUtilities.FindBlockIndex(input.X, y, input.Z); index >= 0; index -= 256, y--, checkBlock = false)
                {
                    BlockGrouping grouping = provider.ProvideGrouping(index);
                    if (grouping.Type == BlockType.StopAtEncounter) 
                    {
                        bool continueScanning = AddCurrentBlockSpan(lastBlock);
                        if (!continueScanning) return;

                        continueScanning = input.BlockOutput.GiveBlock(new ScannedBlock((short)y, provider.ProvideBlockData(index, isTop)), lastBlock);
                        if (!continueScanning) return;

                        isTop = false;
                        continue;
                    }

                    if (grouping.Type == BlockType.Disabled) continue;

                    bool blockSpanNeedsToBeAdded;
                    if (currentScannedBlock.IsEmpty())
                    {
                        currentScannedBlock = new ScannedBlock((short)y, provider.ProvideBlockData(index, isTop));
                        currentCount = 0;
                        blockSpanNeedsToBeAdded = false;

                        lastBlock = provider.ProvideBlock(index);
                        lastGrouping = grouping;

                        isTop = false;
                    }
                    else 
                    {
                        if ((currentScannedBlock.FirstInstanceY - currentCount + 1) > y + 1) blockSpanNeedsToBeAdded = true;
                        else if (checkBlock)
                        {
                            if (lastBlock.Equals(provider.ProvideBlock(index))) blockSpanNeedsToBeAdded = false;
                            else blockSpanNeedsToBeAdded = GroupingChanged();
                        }
                        else blockSpanNeedsToBeAdded = (currentScannedBlock.Data.IndexInBlockPalette != provider.ProvideIndexInPalette(index)) && GroupingChanged();

                        bool GroupingChanged() => !lastGrouping.Equals(grouping);
                    }
                    currentCount++;

                    if (blockSpanNeedsToBeAdded)
                    {
                        bool continueScanning = AddCurrentBlockSpan(lastBlock);
                        if (!continueScanning) return;

                        currentScannedBlock = new ScannedBlock((short)y, provider.ProvideBlockData(index, isTop));
                        currentCount = 1;

                        lastBlock = provider.ProvideBlock(index);
                        lastGrouping = grouping;

                        isTop = false;
                        continue;
                    }
                    
                    if (currentCount >= lastGrouping.ExitOnDepth && !AddCurrentBlockSpan(lastBlock, true)) return;
                }

                checkBlock = true;
            }

            AddCurrentBlockSpan(lastBlock);

            bool AddCurrentBlockSpan(Block block, bool isMax = false)
            {
                if (currentCount == 0) return true;

                bool contineScanning = input.BlockOutput.GiveBlockSpan(new BlockSpan(currentScannedBlock, (short)currentCount), block, isMax);
                currentCount = 0;

                return contineScanning;
            };
        }

        private static void ProvideY(ColumnScanArgs input, out int y, out int sectionY) 
        {
            y = input.LevelProvider.Provide(input.X, input.Z);

            sectionY = MathUtilities.FindSectionY(y);
            if (sectionY > input.SectionCollection.HighestSectionY)
            {
                sectionY = input.SectionCollection.HighestSectionY;
                y = input.SectionCollection.HighestSectionY * 16 + 15;
            }
        }
    }
}
