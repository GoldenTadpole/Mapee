using AssetSystem;
using MapScanner;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class SharedColumnScanArgsFactory : ColumnScanArgsFactory
    {
        public ScanType ScanType { get; set; }

        public SharedColumnScanArgsFactory(IAsset<Block, BlockGrouping> asset) : base(asset) { }

        protected override IBlockOutput CreateBlockOutput(ColumnScanArgsFactoryArgs args)
        {
            IColumnObjectBuilder columnObjectBuilder = new ColumnObjectBuilder(args.ScannedChunk.UniqueColumns, args.ScannedChunk.Indexes);

            switch (ScanType) 
            {
                case ScanType.Cave:
                    return new CaveBlockOutput(columnObjectBuilder);
                default:
                    return new StandardBlockOutput(columnObjectBuilder);
            }
        }
    }
}
