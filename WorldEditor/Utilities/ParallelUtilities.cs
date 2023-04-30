namespace WorldEditor
{
    public static class ParallelUtilities
    {
        public static void BufferedFor(int from, int toExclusive, int buffer, Action<int, int> body)
        {
            int iterations = (int)Math.Ceiling((toExclusive - from) / (float)buffer);

            for (int i = 0; i < iterations; i++)
            {
                int toExclusiveThis = Math.Min((i + 1) * buffer, toExclusive);
                Parallel.For(i * buffer, toExclusiveThis, ii => body(ii, ii - (i * buffer)));
            }
        }
    }
}
