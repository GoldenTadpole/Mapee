using NbtEditor;

namespace MapScanner
{
    public interface IColumnObjectBuilder
    {
        void SetColumnType(ColumnType type);

        void AddBlockSpan(BlockSpan span);
        void AddBottomBlock(ScannedBlock block);

        void EndColumn();
    }
}
