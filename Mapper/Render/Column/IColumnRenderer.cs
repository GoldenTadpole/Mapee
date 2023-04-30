namespace Mapper
{
    public interface IColumnRenderer<TInput>
    {
        VecRgb Render(TInput input);
    }
}
