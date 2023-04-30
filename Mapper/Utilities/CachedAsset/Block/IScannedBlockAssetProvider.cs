using MapScanner;

namespace Mapper
{
    public interface IScannedBlockAssetProvider<TAsset> where TAsset : struct
    {
        TAsset Provide(ScannedBlock block);
        void ResetColumn();
    }
}
