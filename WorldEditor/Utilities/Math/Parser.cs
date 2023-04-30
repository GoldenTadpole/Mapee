using NbtEditor;

namespace WorldEditor
{
    public static class Parser
    {
        public static StorageFormat ParseStorageFormat(string filename)
        {
            return Path.GetExtension(filename) == ".mca" ? StorageFormat.Anvil : StorageFormat.McRegion;
        }
        public static void ParseRegionName(string filename, out int x, out int z)
        {
            string[] split = filename.Substring(2, filename.Length - 6).Split(".");

            x = int.Parse(split[0]);
            z = int.Parse(split[1]);
        }
        public static bool TryParseRegionName(string filename, out int x, out int z)
        {
            try
            {
                ParseRegionName(filename, out x, out z);
                return true;
            }
            catch
            {
                x = 0;
                z = 0;
                return false;
            }
        }
        public static int ParseInt24(byte[] bytes, int offset)
        {
            return bytes[offset] << 16 | bytes[offset + 1] << 8 | bytes[offset + 2];
        }
        public static int ParseInt32(byte[] bytes, int offset)
        {
            return bytes[offset] << 24 | bytes[offset + 1] << 16 | bytes[offset + 2] << 8 | bytes[offset + 3];
        }
        public static short[] ParseValues(Tag arrayTag)
        {
            if(arrayTag is not ArrayTag array) return Array.Empty<short>();
            short[] output = new short[array.InternalArary.Length];

            if (arrayTag.Id == TagId.SignedByteArray)
            {
                sbyte[] source = arrayTag;

                for (int i = 0; i < source.Length; i++)
                {
                    output[i] = source[i];
                }
            }
            else if (arrayTag.Id == TagId.Int32Array)
            {
                int[] source = arrayTag;

                for (int i = 0; i < source.Length; i++)
                {
                    output[i] = (short)source[i];
                }
            }

            return output;
        }
    }
}
