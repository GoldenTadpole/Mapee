namespace Mapper.Gui.Logic
{
    public class SlimeChunkChecker : ISlimeChunkChecker
    {
        public ProgramDomain Domain { get; }

        public SlimeChunkChecker(ProgramDomain domain) 
        {
            Domain = domain;
        }

        public bool IsSlimeChunk(int x, int z)
        {
            if (Domain.CurrentWorld is null) return false;
            long levelSeed = Domain.CurrentWorld.Level.WorldGen.Seed;

            ulong seed = (ulong)(levelSeed + (x * x * 0x4c1906) + (x * 0x5ac0db) + z * z * 0x4307a7L + (z * 0x5f24f) ^ 0x3ad8025fL);
            return JavaRandom.NextInt(seed, 10) == 0;
        }
    }
}
