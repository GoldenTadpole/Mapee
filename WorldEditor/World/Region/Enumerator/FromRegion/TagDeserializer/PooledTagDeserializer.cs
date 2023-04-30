using NbtEditor;
using CommonUtilities.Pool;
using System.Buffers;

namespace WorldEditor
{
    public class PooledTagDeserializer : ITagDeserializer
    {
        public virtual NbtReader[] NbtReaders { get; set; }

        public virtual TagDeserializer[] TagDeserializers { get; set; }
        public virtual PooledValueTagAllocation[] ValueTagAllocations { get; set; }
        public virtual PooledArrayTagAllocation[] ArrayTagAllocations { get; set; }
        public virtual IResettablePool<int, ListTag>[] ListPools { get; set; }
        public virtual IResettablePool<CompoundTag>[] CompoundPools { get; set; }

        private IResettablePool<int, sbyte[]>[] _sbytePools;
        private IResettablePool<int, int[]>[] _intPools;
        private IResettablePool<int, long[]>[] _longPools;

        public PooledTagDeserializer(int tasks)
        {
            NbtReaders = new NbtReader[tasks];
            _sbytePools = new ResettableCachedPool<sbyte>[tasks];
            _intPools = new ResettableCachedPool<int>[tasks];
            _longPools = new ResettableCachedPool<long>[tasks];

            for (int i = 0; i < NbtReaders.Length; i++)
            {
                _sbytePools[i] = new ResettableCachedPool<sbyte>();
                _intPools[i] = new ResettableCachedPool<int>();
                _longPools[i] = new ResettableCachedPool<long>();

                NbtReaders[i] = new NbtReader(new FastBufferProvider(Array.Empty<byte>()), new ArrayPoolAllocation()
                {
                    SBytePool = _sbytePools[i],
                    Int32Pool = _intPools[i],
                    Int64Pool = _longPools[i]
                });
            }

            TagDeserializers = new TagDeserializer[tasks];
            ValueTagAllocations = new PooledValueTagAllocation[tasks];
            ArrayTagAllocations = new PooledArrayTagAllocation[tasks];
            ListPools = new IResettablePool<int, ListTag>[tasks];
            CompoundPools = new IResettablePool<CompoundTag>[tasks];

            for (int i = 0; i < TagDeserializers.Length; i++)
            {
                PooledValueTagAllocation valueTagAllocation = new PooledValueTagAllocation();
                PooledArrayTagAllocation arrayTagAllocation = new PooledArrayTagAllocation();
                IResettablePool<CompoundTag> compoundTagPool = new ExpandableObjectPool<CompoundTag>(0, () => new CompoundTag());
                IResettablePool<int, ListTag> listTagPool = new ExpandableObjectPool<int, ListTag>(0, count => new ListTag(TagId.End));

                TagDeserializers[i] = new TagDeserializer()
                {
                    IdTagDeserializer = new IdTagDeserializer(
                        valueTagAllocation,
                        arrayTagAllocation,
                        listTagPool,
                        compoundTagPool
                    )
                };

                ValueTagAllocations[i] = valueTagAllocation;
                ArrayTagAllocations[i] = arrayTagAllocation;
                ListPools[i] = listTagPool;
                CompoundPools[i] = compoundTagPool;
            }
        }

        public virtual CompoundTag? Deserialize(ArraySlice<byte> decompressed, int iterator)
        {
            NbtReader reader = NbtReaders[iterator];
            reader.BufferProvider = new FastBufferProvider(decompressed.Array, decompressed.Position);

            Tag tag = TagDeserializers[iterator].Deserialize(reader);

            ValueTagAllocations[iterator].Reset();
            ArrayTagAllocations[iterator].Reset();
            ListPools[iterator].Reset();
            CompoundPools[iterator].Reset();

            _sbytePools[iterator].Reset();
            _intPools[iterator].Reset();
            _longPools[iterator].Reset();

            if (tag is not CompoundTag compoundTag) return null;
            return compoundTag;
        }
    }

    public class ResettableCachedPool<T> : IResettablePool<int, T[]>
    {
        public ArrayPool<T> Instance { get; set; }

        private ISet<T[]> _cache = new HashSet<T[]>(0);
        private bool _resetPending = false;

        public ResettableCachedPool()
        {
            Instance = ArrayPool<T>.Shared;
        }

        public T[] Provide(int length)
        {
            if (_resetPending) Reset();

            T[] output = Instance.Rent(length);
            _cache.Add(output);

            return output;
        }

        public void Reset()
        {
            if (!_resetPending)
            {
                _resetPending = true;
                return;
            }

            _resetPending = false;

            foreach (T[] array in _cache)
            {
                Instance.Return(array);
            }

            _cache.Clear();
        }
    }
}
