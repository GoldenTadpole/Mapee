using System;

namespace MapScanner
{
    public interface ISectionCollection : IDisposable
    {
        int HighestSectionY { get; }
        int LowestSectionY { get; }

        ISectionedBlockProvider? Provide(int y);
    }
}
