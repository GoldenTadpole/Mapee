using AssetSystem;
using CommonUtilities.Factory;
using CommonUtilities.Pool;
using WorldEditor;

namespace MapScanner
{
    public class ColumnScanArgsFactory : IFactory<ColumnScanArgsFactoryArgs, ColumnScanArgs>
    {
        public IAsset<Block, BlockGrouping> Asset { get; set; }

        public bool UseHeightmap { get; set; } = true;
        public string Heightmap { get; set; } = "WORLD_SURFACE";
        public short SetY { get; set; } = 319;

        public IPool<IResettablePool<short[]>> Pools { get; set; }

        public ColumnScanArgsFactory(IAsset<Block, BlockGrouping> asset)
        {
            Asset = asset;
            ExpandableIndexedPool<IResettablePool<short[]>> indexedPool = new ExpandableIndexedPool<IResettablePool<short[]>>(32, i => new ExpandableObjectPool<short[]>(10, () => new short[4096]));
            Pools = new ThreadedPool<int, IResettablePool<short[]>>("ThreadIndex", indexedPool);
        }

        public virtual ColumnScanArgs Create(ColumnScanArgsFactoryArgs args)
        {
            ISectionCollection sectionCollection = CreateSectionCollection(args);
            ILevelProvider levelProvider = CreateLevelProvider(args);
            IBlockOutput blockOutput = CreateBlockOutput(args);

            return new ColumnScanArgs(sectionCollection, levelProvider, blockOutput);
        }

        protected virtual ISectionCollection CreateSectionCollection(ColumnScanArgsFactoryArgs args) 
        {
            return new SectionCollection(Asset, args.ApiChunk, Pools.Provide());
        }
        protected virtual ILevelProvider CreateLevelProvider(ColumnScanArgsFactoryArgs args) 
        {
            if (UseHeightmap && args.ApiChunk.Heightmap is not null && args.ApiChunk.Heightmap is HeightmapCollection collection &&
                collection.Heightmaps.TryGetValue(Heightmap, out HeightmapCollection.Heightmap? heightmap))
            {
                return new HeightmapLevelProvider(heightmap);
            }
            else
            {
                return new SimpleLevelProvider(SetY);
            }
        }
        protected virtual IBlockOutput CreateBlockOutput(ColumnScanArgsFactoryArgs args)
        {
            return new StandardBlockOutput(new ColumnObjectBuilder(args.ScannedChunk.UniqueColumns, args.ScannedChunk.Indexes));
        }
    }
}
