namespace Mapper.Gui.Model
{
    public interface ITool : ICanBeEnabled
    {
        event EnableEvent OnEnabled;
    }
}
