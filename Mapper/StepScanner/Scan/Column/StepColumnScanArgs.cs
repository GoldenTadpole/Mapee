using MapScanner;

namespace Mapper
{
    public class StepColumnScanArgs
    {
        public ScannedColumn Column { get; private set; }
        public IScannedBlockAssetProvider<StepType>? StepTypeAssetProvider { get; init; }

        public StepColumnScanArgs Copy(ScannedColumn column)
        {
            Column = column;
            return this;
        }
    }
}
