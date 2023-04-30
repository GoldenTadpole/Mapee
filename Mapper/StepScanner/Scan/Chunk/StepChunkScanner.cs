using AssetSystem;
using MapScanner;
using WorldEditor;

namespace Mapper
{
    public class StepChunkScanner : IObjectScanner<IScannedChunk, StepChunk>
    {
        public IObjectScanner<StepColumnScanArgs, short> StepColumnScanner { get; set; }
        public IAsset<Block, StepType> Asset { get; set; }

        public StepChunkScanner(IAsset<Block, StepType> asset) : this(new StepColumnScanner(), asset) { }
        public StepChunkScanner(IObjectScanner<StepColumnScanArgs, short> columnScanner, IAsset<Block, StepType> asset)
        {
            StepColumnScanner = columnScanner;
            Asset = asset;
        }

        public StepChunk Scan(IScannedChunk chunk)
        {
            StepColumnScanArgs scanParameter = CreateReadParameter(chunk);

            Span<short> uniqueValues = stackalloc short[chunk.UniqueColumns.Count];
            for (int i = 0; i < uniqueValues.Length; i++)
            {
                scanParameter.StepTypeAssetProvider?.ResetColumn();

                uniqueValues[i] = StepColumnScanner.Scan(scanParameter.Copy(chunk.UniqueColumns[i]));
            }

            StepChunk output = new StepChunk();
            for (int i = 0; i < 256; i++)
            {
                scanParameter.StepTypeAssetProvider?.ResetColumn();

                output.Steps[i] = uniqueValues[chunk.Indexes[i]];
            }

            return output;
        }

        private StepColumnScanArgs CreateReadParameter(IScannedChunk chunk)
        {
            if (chunk.BlockSections is null) throw new ArgumentNullException();

            CachedAssetProvider<StepType> provider = new(Asset, chunk.BlockSections);

            return new StepColumnScanArgs()
            {
                StepTypeAssetProvider = provider
            };
        }
    }
}
