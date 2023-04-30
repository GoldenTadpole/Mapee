using System.Buffers.Binary;
using System.Text;

namespace NbtEditor
{
    public class NbtWriter : INbtWriter 
    {
        public BufferWriter BufferWriter { get; set; }
        public bool IsFormatLittleEndian { get; set; } = false;

        public NbtWriter(BufferWriter bufferWriter)
        {
            BufferWriter = bufferWriter;
        }

        public void WriteSignedByte(sbyte value) 
        {
            BufferWriter.WriteBuffer(stackalloc byte[sizeof(sbyte)] { (byte) value });
        }
        public void WriteInt16(short value)
        {
            if(IsInReverse()) value = BinaryPrimitives.ReverseEndianness(value);

            Span<byte> buffer = stackalloc byte[sizeof(short)];
            if (!BitConverter.TryWriteBytes(buffer, value)) return;

            BufferWriter.WriteBuffer(buffer);
        }
        public void WriteUnsignedInt16(ushort value) 
        {
            if (IsInReverse()) value = BinaryPrimitives.ReverseEndianness(value);

            Span<byte> buffer = stackalloc byte[sizeof(ushort)];
            if (!BitConverter.TryWriteBytes(buffer, value)) return;

            BufferWriter.WriteBuffer(buffer);
        }
        public void WriteInt32(int value) 
        {
            if (IsInReverse()) value = BinaryPrimitives.ReverseEndianness(value);

            Span<byte> buffer = stackalloc byte[sizeof(int)];
            if (!BitConverter.TryWriteBytes(buffer, value)) return;

            BufferWriter.WriteBuffer(buffer);
        }
        public void WriteInt64(long value) 
        {
            if (IsInReverse()) value = BinaryPrimitives.ReverseEndianness(value);

            Span<byte> buffer = stackalloc byte[sizeof(long)];
            if (!BitConverter.TryWriteBytes(buffer, value)) return;

            BufferWriter.WriteBuffer(buffer);
        }
        public void WriteSingle(float value)
        {
            Span<byte> buffer = stackalloc byte[sizeof(long)];
            if (!BitConverter.TryWriteBytes(buffer, value)) return;

            if (IsInReverse()) buffer = stackalloc byte[sizeof(float)] {
                    buffer[3], buffer[2], buffer[1], buffer[0] };

            BufferWriter.WriteBuffer(buffer);
        }
        public void WriteDouble(double value)
        {
            Span<byte> buffer = stackalloc byte[sizeof(double)];
            if (!BitConverter.TryWriteBytes(buffer, value)) return;

            if (IsInReverse()) buffer = stackalloc byte[sizeof(double)] {
                buffer[7], buffer[6], buffer[5], buffer[4],
                buffer[3], buffer[2], buffer[1], buffer[0] };

            BufferWriter.WriteBuffer(buffer);
        }
        public void WriteString(string value)
        {
            int byteCount = Encoding.UTF8.GetByteCount(value);
            Span<byte> buffer = stackalloc byte[byteCount];
            Encoding.UTF8.GetBytes(value, buffer);

            WriteUnsignedInt16((ushort)buffer.Length);
            BufferWriter.WriteBuffer(buffer);
        }
        public void WriteSignedByteArray(sbyte[] array)
        {
            BufferWriter.WriteBuffer((byte[]) (Array) array );
        }
        public void WriteInt32Array(int[] array)
        {
            for (int i = 0; i < array.Length; i++) 
            {
                WriteInt32(array[i]);
            }
        }
        public void WriteInt64Array(long[] array)
        {
            for (int i = 0; i < array.Length; i++) 
            {
                WriteInt64(array[i]);
            }
        }

        private bool IsInReverse()
        {
            return IsFormatLittleEndian != BitConverter.IsLittleEndian;
        }
    }
}
