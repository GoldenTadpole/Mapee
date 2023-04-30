namespace NbtEditor 
{
    public sealed class ArrayTag : Tag
    {
        public Array InternalArary { get; set; }

        public static implicit operator ArrayTag(sbyte[] array) => new(array);
        public static implicit operator ArrayTag(int[] array) => new(array);
        public static implicit operator ArrayTag(long[] array) => new(array);

        public static implicit operator sbyte[](ArrayTag array) => (sbyte[])array.InternalArary;
        public static implicit operator int[](ArrayTag array) => (int[])array.InternalArary;
        public static implicit operator long[](ArrayTag array) => (long[])array.InternalArary;

        public ArrayTag(sbyte[] array) : base(TagId.SignedByteArray)
        {
            InternalArary = array;
        }
        public ArrayTag(int[] array) : base(TagId.Int32Array)
        {
            InternalArary = array;
        }
        public ArrayTag(long[] array) : base(TagId.Int64Array)
        {
            InternalArary = array;
        }
        public ArrayTag() : base(TagId.SignedByte) 
        {
            InternalArary = null;
        }

        public void ChangeId(TagId id)
        {
            Id = id;
        }
    }
}
