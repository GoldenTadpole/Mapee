namespace Mapper
{
    public interface IStepProvider
    {
        short[]? ProvideStepStrip(int x, int z, Direction direction);
        short[]? ProvideStepChunk(int x, int z);
    }
}
