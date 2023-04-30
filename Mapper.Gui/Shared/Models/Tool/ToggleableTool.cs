namespace Mapper.Gui.Model
{
    public class ToggleableTool : IToggleableTool
    {
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled == value) return;

                _enabled = value;
                OnEnabled?.Invoke(value);
            }
        }
        private bool _enabled = true;

        public bool IsTurnedOn
        {
            get => _isTurnedOn;
            set
            {
                if (_isTurnedOn == value) return;

                _isTurnedOn = value;
                OnTurnedOn?.Invoke(value);
            }
        }
        private bool _isTurnedOn = false;

        public event EnableEvent? OnEnabled;
        public event ToggleEvent? OnTurnedOn;
    }
}
