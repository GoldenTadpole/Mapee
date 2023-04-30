using CommonUtilities.Factory;

namespace Mapper
{
    public class ChunkBlockControllerFactory : IFactory<ChunkRenderArgs, IBlockController>
    {
        public AssetPack AssetPack { get; set; }

        public ChunkBlockControllerFactory(AssetPack assetPack) 
        {
            AssetPack = assetPack;
        }

        public IBlockController Create(ChunkRenderArgs input)
        {
            return new BlockController(input.ScannedChunk, AssetPack, input.StepProvider);
        }
    }
}
