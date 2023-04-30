using CommonUtilities.Collections.Simple;
using System;
using System.Collections.Generic;

namespace MapScanner
{
    public class ColumnObjectBuilder : IColumnObjectBuilder
    {
        public IList<ScannedColumn> Cache { get; set; }
        public byte[] Indexes { get; set; }

        private const int _cacheLength = 64;
        private Dictionary<int, byte> _cache = new Dictionary<int, byte>(_cacheLength);

        private ColumnType _columnType = ColumnType.Empty;
        private SimpleList<BlockSpan> _blockSpans = new();
        private ScannedBlock _bottomBlock = new ScannedBlock();
        private int _index = 0;

        public ColumnObjectBuilder(IList<ScannedColumn> cache, byte[] indexes)
        {
            Cache = cache;
            Indexes = indexes;
        }

        public void SetColumnType(ColumnType columnType)
        {
            _columnType = columnType;
            _blockSpans.Clear();
            _bottomBlock = new ScannedBlock();
        }

        public void AddBlockSpan(BlockSpan blockSpan)
        {
            _blockSpans.Add(blockSpan);
        }
        public void AddBottomBlock(ScannedBlock block)
        {
            _bottomBlock = block;
        }

        public void EndColumn()
        {
            int hash = ScannedColumn.GetHashCode(_blockSpans.InternalArray, _bottomBlock);

            if (_cache.TryGetValue(hash, out byte index))
            {
                Indexes[_index] = index;
            }
            else
            {
                index = (byte)Cache.Count;

                if (_cache.Count < _cacheLength)
                {
                    _cache.Add(hash, index);
                }

                Cache.Add(CreateColumn());

                Indexes[_index] = index;
            }

            _index++;
        }

        private ScannedColumn CreateColumn()
        {
            switch (_columnType)
            {
                case ColumnType.StopAtEncounter:
                    return new ScannedColumn(_bottomBlock);
                case ColumnType.SemiTransparent:
                    ScannedColumn output = new ScannedColumn(new Memory<BlockSpan>(_blockSpans.InternalArray, 0, _blockSpans.Count), _bottomBlock);
                    _blockSpans = new SimpleList<BlockSpan>();

                    return output;
                default:
                    return ScannedColumn.Empty;
            }
        }
    }
}
