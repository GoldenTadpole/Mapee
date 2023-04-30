using WorldEditor;

namespace MapScanner
{
    public class CaveBlockOutput : IBlockOutput
    {
        public IColumnObjectBuilder Builder { get; set; }

        private bool _isValidColumn = false;
        private bool _added = false;

        private const string AIR_NAMESPACE = "minecraft:air";
        private const string CAVE_AIR_NAMESPACE = "minecraft:cave_air";

        public CaveBlockOutput(IColumnObjectBuilder builder)
        {
            Builder = builder;
        }

        public bool GiveBlock(ScannedBlock block, Block representedBlock)
        {
            if (_isValidColumn)
            {
                if (!_added) Builder.SetColumnType(ColumnType.StopAtEncounter);

                Builder.AddBottomBlock(block);
                return false;
            }

            return true;
        }
        public bool GiveBlockSpan(BlockSpan span, Block representedBlock, bool isMaxReached = false)
        {
            if (!_isValidColumn)
            {
                if (representedBlock.Name == AIR_NAMESPACE || representedBlock.Name == CAVE_AIR_NAMESPACE)
                {
                    _isValidColumn = true;
                    Builder.SetColumnType(ColumnType.SemiTransparent);
                    return true;
                }
            }
            else
            {
                if (representedBlock.Name != AIR_NAMESPACE && representedBlock.Name != CAVE_AIR_NAMESPACE)
                {
                    Builder.AddBlockSpan(span);
                    _added = true;
                }
            }

            return true;
        }

        public void BeginScan()
        {
            _isValidColumn = false;
            _added = false;
        }
        public void EndScan()
        {
            if (!_isValidColumn) Builder.SetColumnType(ColumnType.Empty);

            Builder.EndColumn();
        }
    }
}
