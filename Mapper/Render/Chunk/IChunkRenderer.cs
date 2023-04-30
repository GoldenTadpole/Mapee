namespace Mapper
{
    public interface IChunkRenderer<TInput>
    {
        void Render(TInput input, ICanvas canvas);
    }
}
