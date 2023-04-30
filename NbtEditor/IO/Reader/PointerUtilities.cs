namespace NbtEditor 
{
    public static class PointerUtilities
    {
        public static unsafe short ReadInt16(byte* b, bool smallEndian = false) 
        {
            if (CanCast(smallEndian)) return *(short*)b;
            return (short)(*b << 8 | *(b + 1));
        }
        public static unsafe ushort ReadUnsignedInt16(byte* b, bool smallEndian = false)
        {
            if (CanCast(smallEndian)) return *(ushort*)b;
            return (ushort)(*b << 8 | *(b + 1));
        }
        public static unsafe int ReadInt32(byte* b, bool smallEndian = false) 
        {
            if (CanCast(smallEndian)) return *(int*)b;
            return (*b << 24) | (*(b + 1) << 16) | (*(b + 2) << 8) | (*(b + 3));
        }
        public static unsafe long ReadInt64(byte* b, bool smallEndian = false) 
        {
            if (CanCast(smallEndian)) return *(long*)b;

            int i = ((*b) << 24) | (*(b + 1) << 16) | (*(b + 2) << 8) | (*(b + 3));
            int j = ((*(b + 4)) << 24) | (*(b + 5) << 16) | (*(b + 6) << 8) | (*(b + 7));

            return (long)((*(ulong*)&i << 32) | *(ulong*)&j);
        }
        public static unsafe float ReadSingle(byte* b, bool smallEndian = false) 
        {
            if (CanCast(smallEndian)) return *(float*)b;

            int i = (*b << 24) | (*(b + 1) << 16) | (*(b + 2) << 8) | (*(b + 3));
            return *(float*)&i;
        }
        public static unsafe double ReadDouble(byte* b, bool smallEndian = false) 
        {
            if (CanCast(smallEndian)) return *(double*)b;

            int i = ((*b) << 24) | (*(b + 1) << 16) | (*(b + 2) << 8) | (*(b + 3));
            int j = ((*(b + 4)) << 24) | (*(b + 5) << 16) | (*(b + 6) << 8) | (*(b + 7));
            ulong k = (*(ulong*)&i << 32) | *(ulong*)&j;

            return *(double*)&k;
        }

        public static unsafe void ReadInt32Array(byte* b, int[] output, bool smallEndian = false)
        {
            if (CanCast(smallEndian)) 
            {
                fixed (int* o = output) 
                {
                    Buffer.MemoryCopy(b, o, output.Length * sizeof(int), output.Length * sizeof(int));
                }
            } 
            else 
            {
                for (int i = 0; i < output.Length; i++) 
                {
                    int offset = i * sizeof(int);

                    output[i] = (*(b + offset) << 24) | (*(b + offset + 1) << 16) | (*(b + offset + 2) << 8) | (*(b + offset + 3));
                }
            }
        }
        public static unsafe void ReadInt64Array(byte* b, long[] output, bool smallEndian = false) 
        {
            if (CanCast(smallEndian))
            {
                fixed (long* o = output) 
                {
                    Buffer.MemoryCopy(b, o, output.Length * sizeof(long), output.Length * sizeof(long));
                }
            } 
            else
            {
                for (int i = 0; i < output.Length; i++) 
                {
                    int offset = i * sizeof(long);

                    int j = ((*(b + offset)) << 24) | (*(b + offset + 1) << 16) | (*(b + offset + 2) << 8) | (*(b + offset + 3));
                    int k = ((*(b + offset + 4)) << 24) | (*(b + offset + 5) << 16) | (*(b + offset + 6) << 8) | (*(b + offset + 7));

                    output[i] = (long)((*(ulong*)&j << 32) | *(ulong*)&k);
                }
            }
        }

        private static bool CanCast(bool smallEndian) 
        {
            return smallEndian == BitConverter.IsLittleEndian;
        }
    }
}
