namespace NbtEditor 
{
    public interface INbtWriter
    {
        void WriteSignedByte(sbyte value);
        void WriteInt16(short value);
        void WriteUnsignedInt16(ushort value);
        void WriteInt32(int value);
        void WriteInt64(long value);
        void WriteSingle(float value);
        void WriteDouble(double value);
        void WriteString(string value);
        void WriteSignedByteArray(sbyte[] array);
        void WriteInt32Array(int[] array);
        void WriteInt64Array(long[] array);
    }
}
