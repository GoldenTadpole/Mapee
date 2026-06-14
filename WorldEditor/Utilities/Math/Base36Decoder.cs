namespace WorldEditor
{
    public static class Base36Decoder
    {
        private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static long Decode(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be null or empty.");
            }

            input = input.ToUpperInvariant();

            bool isNegative = input[0] == '-';
            if (isNegative)
            {
                if (input.Length == 1)
                    throw new FormatException("Input cannot be a lone minus sign.");

                input = input.Substring(1);
            }

            long result = 0;
            long multiplier = 1;

            for (int i = input.Length - 1; i >= 0; i--)
            {
                int digit = Alphabet.IndexOf(input[i]);
                if (digit < 0)
                {
                    throw new FormatException($"Invalid Base36 character: '{input[i]}'");
                }

                result += digit * multiplier;
                multiplier *= 36;
            }

            return isNegative ? -result : result;
        }
    }
}
