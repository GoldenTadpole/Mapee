using WorldEditor;

namespace MapScanner
{
    public class StandardBlockOutput : IBlockOutput
    {
        public IColumnObjectBuilder Builder { get; private set; }

        private bool _isNew = false;
        private bool _solidPlaced = false;

        public StandardBlockOutput(IColumnObjectBuilder builder)
        {
            Builder = builder;
        }

        public bool GiveBlock(ScannedBlock block, Block representedBlock)
        {
            if (_isNew)
            {
                Builder.SetColumnType(ColumnType.StopAtEncounter);
                _isNew = false;
            }

            Builder.AddBottomBlock(block);
            _solidPlaced = true;

            return false;
        }
        public bool GiveBlockSpan(BlockSpan span, Block representedBlock, bool isMaxReached = false)
        {
            if (_isNew)
            {
                Builder.SetColumnType(ColumnType.SemiTransparent);
                _isNew = false;
            }

            Builder.AddBlockSpan(span);

            return !isMaxReached;
        }

        public void BeginScan()
        {
            _isNew = true;
            _solidPlaced = false;
        }
        public void EndScan()
        {
            if (_isNew) Builder.SetColumnType(ColumnType.Empty);
            else if (!_solidPlaced) Builder.AddBottomBlock(ScannedBlock.Empty);

            Builder.EndColumn();
        }
    }
}
