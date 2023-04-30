using System.Globalization;

namespace Mapper.Gui
{
    public static class InputValidator
    {
        public static bool ValidateInt32(string text, int min = int.MinValue, int max = int.MaxValue) 
        {
            if (!int.TryParse(text, out int value))
            {
                return false;
            }

            return value >= min && value <= max;
        }
        public static bool ValidateInt16(string text, short min = short.MinValue, short max = short.MaxValue)
        {
            if (!short.TryParse(text, out short value))
            {
                return false;
            }

            return value >= min && value <= max;
        }
        public static bool ValidateSingle(string text, float min = float.MinValue, float max = float.MaxValue)
        {
            if(!float.TryParse(text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out float value)) 
            {
                return false;
            }

            return value >= min && value <= max;
        }
    }
}
