using System.Diagnostics.CodeAnalysis;

namespace CommonUtilities.Collections.Observable
{
    public class ObservableDictionary<TKey, TValue> : ObservableCollection<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue> where TKey : notnull
    {
        protected IDictionary<TKey, TValue> BaseDictionary { get; }

        public ICollection<TKey> Keys => BaseDictionary.Keys;
        public ICollection<TValue> Values => BaseDictionary.Values;

        public TValue this[TKey key]
        {
            get => BaseDictionary[key];
            set 
            {
                bool exists = ReferenceEquals(BaseDictionary[key], value);
                BaseDictionary[key] = value;

                if (!exists) InvokeEvent();
            }
        }

        public ObservableDictionary() : this(new Dictionary<TKey, TValue>()) { }
        public ObservableDictionary(int capacity) : this(new Dictionary<TKey, TValue>(capacity)) { }
        public ObservableDictionary(IDictionary<TKey, TValue> baseDictionary) : base(baseDictionary)
        {
            BaseDictionary = baseDictionary;
        }

        public void Add(TKey key, TValue value)
        {
            BaseDictionary.Add(key, value);
            InvokeEvent();
        }

        public bool ContainsKey(TKey key)
        {
            return BaseDictionary.ContainsKey(key);
        }
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return BaseDictionary.TryGetValue(key, out value);
        }

        public bool Remove(TKey key)
        {
            bool exists = BaseDictionary.Remove(key);
            if (exists) InvokeEvent();

            return exists;
        }
    }
}
