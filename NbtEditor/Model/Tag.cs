namespace NbtEditor 
{
    public abstract class Tag {
        public TagId Id { get; protected set; }

        public static implicit operator Tag(sbyte value) => new SignedByteTag(value);
        public static implicit operator Tag(short value) => new Int16Tag(value);
        public static implicit operator Tag(int value) => new Int32Tag(value);
        public static implicit operator Tag(long value) => new Int64Tag(value);
        public static implicit operator Tag(float value) => new SingleTag(value);
        public static implicit operator Tag(double value) => new DoubleTag(value);
        public static implicit operator Tag(string value) => new StringTag(value);
        public static implicit operator Tag(sbyte[] array) => array;
        public static implicit operator Tag(int[] array) => array;
        public static implicit operator Tag(long[] array) => array;

        public static implicit operator sbyte(Tag tag) => (SignedByteTag)tag;
        public static implicit operator short(Tag tag) => (Int16Tag)tag;
        public static implicit operator int(Tag tag) => (Int32Tag)tag;
        public static implicit operator long(Tag tag) => (Int64Tag)tag;
        public static implicit operator float(Tag tag) => (SingleTag)tag;
        public static implicit operator double(Tag tag) => (DoubleTag)tag;
        public static implicit operator string(Tag tag) => (StringTag)tag;
        public static implicit operator sbyte[](Tag array) => (ArrayTag)array;
        public static implicit operator int[](Tag array) => (ArrayTag)array;
        public static implicit operator long[](Tag array) => (ArrayTag)array;

        public Tag(TagId id)
        {
            Id = id;
        }

        public static Tag FromNbtReader(INbtReader reader)
        {
            return new TagDeserializer().Deserialize(reader);
        }
        public static Tag FromStream(Stream inputStream)
        {
            return FromNbtReader(new UnsafeNbtReader(new StreamBufferProvider(inputStream), ArrayPoolAllocation.CreateDefault()));
        }
        public static Tag FromBytes(byte[] bytes, int index = 0)
        {
            return FromNbtReader(new UnsafeNbtReader(bytes, index));
        }

        public void WriteToStream(Stream outputStream)
        {
            new TagSerializer().Serialize(this, new NbtWriter(new StreamBufferWriter(outputStream)));
        }
        public byte[] ToBytes()
        {
            using MemoryStream outputStream = new MemoryStream();
            WriteToStream(outputStream);
            return outputStream.ToArray();
        }
    }
}
