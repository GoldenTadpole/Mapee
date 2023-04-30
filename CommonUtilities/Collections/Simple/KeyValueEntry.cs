using System.Diagnostics.CodeAnalysis;

namespace CommonUtilities.Collections.Simple
{
    public readonly struct KeyValueEntry<T, U>
    {
        public T Key { get; }
        public U Value { get; }
        public int Hashcode { get; }

        public KeyValueEntry(T key, U value, int hashCode) 
        {
            Key = key;
            Value = value;
            Hashcode = hashCode;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is KeyValueEntry<T, U> entry && entry.Hashcode == Hashcode;
        }
        public override int GetHashCode()
        {
            return Hashcode;
        }
    }
}
