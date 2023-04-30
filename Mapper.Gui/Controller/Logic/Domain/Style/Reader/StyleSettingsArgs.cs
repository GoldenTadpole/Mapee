using CommonUtilities.Data;

namespace Mapper.Gui.Logic
{
    public readonly struct StyleSettingsArgs
    {
        public IDataReader Input { get; init; }
        public Style Output { get; init; }

        public StyleSettingsArgs(IDataReader input, Style output) 
        {
            Input = input;
            Output = output;
        }
    }
}
