namespace Mapper
{
    public interface IMapRenderer<TInput>
    {
        void Render(TInput input, out ICanvas canvas);
    }
}
