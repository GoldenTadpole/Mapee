namespace WorldEditor
{
    public static class VersionExtensions
    {
        private readonly static Version[] _releases = new[] {
            Version.Rel_1_19_2,
            Version.Rel_1_19_1,
            Version.Rel_1_19,
            Version.Rel_1_18_2,
            Version.Rel_1_18_1,
            Version.Rel_1_18,
            Version.Rel_1_17_1,
            Version.Rel_1_17,
            Version.Rel_1_16_5,
            Version.Rel_1_16_4,
            Version.Rel_1_16_3,
            Version.Rel_1_16_2,
            Version.Rel_1_16_1,
            Version.Rel_1_16,
            Version.Rel_1_15_2,
            Version.Rel_1_15_1,
            Version.Rel_1_15,
            Version.Rel_1_14_4,
            Version.Rel_1_14_3,
            Version.Rel_1_14_2,
            Version.Rel_1_14_1,
            Version.Rel_1_14,
            Version.Rel_1_13_2,
            Version.Rel_1_13_1,
            Version.Rel_1_13,
            Version.Rel_1_12_2,
            Version.Rel_1_12_1,
            Version.Rel_1_12,
            Version.Rel_1_11_2,
            Version.Rel_1_11_1,
            Version.Rel_1_11,
            Version.Rel_1_10_2,
            Version.Rel_1_10_1,
            Version.Rel_1_10,
            Version.Rel_1_9_4,
            Version.Rel_1_9_3,
            Version.Rel_1_9_2,
            Version.Rel_1_9_1,
            Version.Rel_1_9
        };

        private readonly static Version[] _array = (Version[])Enum.GetValues(typeof(Version));

        public static Version Prev(this Version value)
        {
            int index = Array.IndexOf(_array, value) - 1;

            return (_array.Length == index) ? _array[0] : _array[index];
        }
        public static Version Next(this Version value)
        {
            int index = Array.IndexOf(_array, value) + 1;

            return (_array.Length == index) ? _array[0] : _array[index];
        }

        public static Version PrevRelease(this Version value)
        {
            int index = Array.IndexOf(_releases, value) - 1;

            return (_releases.Length == index) ? _releases[0] : _releases[index];
        }
        public static Version NextRelease(this Version value)
        {
            int index = Array.IndexOf(_releases, value) + 1;

            return (_releases.Length == index) ? _releases[0] : _releases[index];
        }
    }
}
