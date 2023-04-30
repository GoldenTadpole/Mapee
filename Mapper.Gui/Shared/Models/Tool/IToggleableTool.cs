namespace Mapper.Gui.Model
{
    public interface IToggleableTool : ITool, IToggleable
    {
        event ToggleEvent OnTurnedOn;
    }
}
