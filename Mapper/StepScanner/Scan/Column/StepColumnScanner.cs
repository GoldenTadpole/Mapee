using MapScanner;

namespace Mapper
{
    public class StepColumnScanner : IObjectScanner<StepColumnScanArgs, short>
    {
        public virtual short Scan(StepColumnScanArgs input)
        {
            if (input.StepTypeAssetProvider is null) return short.MinValue;

            switch (input.Column.Type)
            {
                case ColumnType.StopAtEncounter:
                    return ScanStopAtEncounterColumn(input.StepTypeAssetProvider, input.Column);
                case ColumnType.SemiTransparent:
                    return ScanSemiTransparentColumn(input.StepTypeAssetProvider, input.Column);
            }

            return short.MinValue;
        }
        protected virtual short ScanStopAtEncounterColumn(IScannedBlockAssetProvider<StepType> stepTypeAssetProvider, ScannedColumn column)
        {
            return column.BottomBlock.FirstInstanceY;
        }
        protected virtual short ScanSemiTransparentColumn(IScannedBlockAssetProvider<StepType> stepTypeAssetProvider, ScannedColumn column)
        {
            ReadOnlySpan<BlockSpan> span = column.BlockSpans.Span;

            for (int i = 0; i < span.Length; i++)
            {
                BlockSpan blockSpan = span[i];

                if (stepTypeAssetProvider.Provide(blockSpan.Block) == StepType.StopAtEncounter)
                {
                    return blockSpan.TopY;
                }
            }

            if (!column.BottomBlock.IsEmpty() && stepTypeAssetProvider.Provide(column.BottomBlock) == StepType.StopAtEncounter)
            {
                return column.BottomBlock.FirstInstanceY;
            }

            return short.MinValue;
        }
    }
}
