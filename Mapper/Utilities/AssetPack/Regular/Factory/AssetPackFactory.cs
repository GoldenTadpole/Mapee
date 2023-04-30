using AssetSystem;
using AssetSystem.Biome;
using AssetSystem.Block;
using CommonUtilities.Factory;
using CommonUtilities.Data;
using MapScanner;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class AssetPackFactory : IFactory<IDataReader, AssetPack>
    {
        public virtual IObjectReader<byte[]?, AssetInstructions?> AssetInstructionsReader { get; set; } = new AssetInstructionsReader();
        public virtual string InstructionsFile { get; set; } = "Instructions.json";
        public virtual AssetPack Default { get; set; } = new AssetPack();

        public virtual AssetPack Create(IDataReader data)
        {
            AssetInstructions? instructions = AssetInstructionsReader.Read(data.ReadFile(InstructionsFile));
            if (instructions is null) return new AssetPack();

            return new AssetPack()
            {
                BlockGroupingAsset = CreateBlockGroupingAsset(instructions.Value.BlockGrouping, data) ?? TokenizedBlockAsset<BlockGrouping>.Empty,
                BlockColorAsset = CreateBlockColorAsset(instructions.Value.BlockColor, data) ?? Asset<Block, RgbA>.Empty,
                BiomeColorAsset = CreateBiomeColorAsset(instructions.Value.BiomeColor, data) ?? Asset<VecRgb>.Empty,
                ElevationAsset = CreateElevationAsset(instructions.Value.Elevation, data) ?? Asset<ElevationSettings>.Empty,
                DepthOpacityAsset = CreateDepthOpacityAsset(instructions.Value.DepthOpacity, data) ?? Asset<DepthOpacity>.Empty,
                StepTypeAsset = CreateStepTypeAsset(instructions.Value.Step, data) ?? Asset<Block, StepType>.Empty,
                StepSettingsAsset = CreateStepSettingsAsset(instructions.Value.StepSettings, data) ?? Asset<Block, StepSettings>.Empty,
            };
        }

        protected virtual ITokenizedBlockAsset<BlockGrouping>? CreateBlockGroupingAsset(BlockAssetArgs args, IDataReader data)
        {
            TokenAsset<BlockGrouping> reader = new()
            {
                Args = args,
                Data = data,
                Cloneable = Default.BlockGroupingAsset,
                Reader = new BlockGroupingPayloadReader()
            };

            return reader.Read();
        }
        protected virtual IAsset<Block, RgbA>? CreateBlockColorAsset(BlockAssetArgs args, IDataReader data)
        {
            AssetBlock<RgbA> reader = new() 
            {
                Args = args,
                Data = data,
                Cloneable = Default.BlockColorAsset,
                Reader = new RgbAPayloadReader(string.Empty, null)
            };

            return reader.Read();
        }
        protected virtual IBiomeAsset<VecRgb>? CreateBiomeColorAsset(BiomeAssetArgs args, IDataReader data)
        {
            AssetBiome<VecRgb> reader = new()
            {
                Args = args,
                Data = data,
                Cloneable = Default.BiomeColorAsset,
                Reader = new VecRgbBiomePayloadReader(string.Empty, null)
            };

            return reader.Read();
        }
        protected virtual IBiomeAsset<ElevationSettings>? CreateElevationAsset(BiomeAssetArgs args, IDataReader data)
        {
            ElevationPayloadReader payloadReader = new();
            AssetComposite<ElevationSettings> reader = new()
            {
                Args = args,
                Data = data,
                Cloneable = Default.ElevationAsset,
                BlockReader = payloadReader,
                BiomeReader = payloadReader
            };

            payloadReader.Default = payloadReader.Read(new BlockReadArgs(args.Block.Args, string.Empty, null)) ?? default;

            return reader.Read();
        }
        protected virtual IBiomeAsset<DepthOpacity>? CreateDepthOpacityAsset(BiomeAssetArgs args, IDataReader data)
        {
            DepthOpacityPayloadReader payloadReader = new();
            AssetComposite<DepthOpacity> reader = new()
            {
                Args = args,
                Data = data,
                Cloneable = Default.DepthOpacityAsset,
                BlockReader = payloadReader,
                BiomeReader = payloadReader
            };

            payloadReader.Default = payloadReader.Read(new BlockReadArgs(args.Block.Args, string.Empty, null)) ?? default;

            return reader.Read();
        }
        protected virtual IAsset<Block, StepType>? CreateStepTypeAsset(BlockAssetArgs args, IDataReader data)
        {
            AssetBlock<StepType> reader = new()
            {
                Args = args,
                Data = data,
                Cloneable = Default.StepTypeAsset,
                Reader = new StepPayloadReader()
            };

            return reader.Read();
        }
        protected virtual IAsset<Block, StepSettings>? CreateStepSettingsAsset(BlockAssetArgs args, IDataReader data)
        {
            StepSettingsPayloadReader payloadReader = new();
            AssetBlock<StepSettings> reader = new()
            {
                Args = args,
                Data = data,
                Cloneable = Default.StepSettingsAsset,
                Reader = new StepSettingsPayloadReader()
            };

            payloadReader.Default = payloadReader.Read(new BlockReadArgs(args.Args, string.Empty, null)) ?? default;

            return reader.Read();
        }
    }
}