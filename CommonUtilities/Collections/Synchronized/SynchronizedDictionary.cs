using System.Diagnostics.CodeAnalysis;

namespace CommonUtilities.Collections.Synchronized
{
    public class SynchronizedDictionary<TKey, TValue> : SynchronizedCollection<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue> where TKey : notnull
    {
        protected IDictionary<TKey, TValue> Dictionary { get; }

        public ICollection<TKey> Keys 
        {
            get
            {
                lock (Root)
                {
                    return Dictionary.Keys;
                }
            }
        }
        public ICollection<TValue> Values
        {
            get
            {
                lock (Root)
                {
                    return Dictionary.Values;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (Root)
                {
                    return Dictionary[key];
                }
            }
            set
            {
                lock (Root)
                {
                    Dictionary[key] = value;
                }
            }
        }

        public SynchronizedDictionary() : this(new Dictionary<TKey, TValue>()) {}
        public SynchronizedDictionary(int capacity) : this(new Dictionary<TKey, TValue>(capacity)){}
        public SynchronizedDictionary(IDictionary<TKey, TValue> baseDictionary) : base(baseDictionary, new object())
        {
            Dictionary = baseDictionary;
        }

        public void Add(TKey key, TValue value)
        {
            lock (Root)
            {
                Dictionary.Add(key, value);
            }
        }
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            lock (Root)
            {
                return Dictionary.TryGetValue(key, out value);
            }
        }
        public bool ContainsKey(TKey key)
        {
            lock (Root)
            {
                return Dictionary.ContainsKey(key);
            }
        }
        public bool Remove(TKey key)
        {
            lock (Root) 
            {
                return Dictionary.Remove(key);
            }
        }
    }
}
