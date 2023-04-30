using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CommonUtilities.Collections.Simple
{
    public class SimpleHashedList<T, U> : IEnumerable<KeyValueEntry<T, U>> where T : notnull
    {
        private readonly SimpleList<KeyValueEntry<T, U>> _internalList;

        public ushort Count => (ushort)_internalList.Count;

        public U? this[T key]
        {
            get
            {
                TryGetValue(key.GetHashCode(), out U? value);
                return value;
            }
            set 
            {
                if(value is null) throw new ArgumentNullException(nameof(value));
                SetValue(key.GetHashCode(), value);
            }
        }

        public SimpleHashedList(int count = 0)
        {
            _internalList = new SimpleList<KeyValueEntry<T, U>>(count);
        }

        public void Add(T key, U value)
        {
            int hashCode = key.GetHashCode();

            KeyValueEntry<T, U> entry = new (key, value, hashCode);
            _internalList.Add(entry);
        }
        public void Clear()
        {
            _internalList.Clear();
        }
        public void ClearInternalArray()
        {
            _internalList.ClearInternalArray();
        }

        public void Remove(T key)
        {
            int hashCode = key.GetHashCode();

            for (int i = 0; i < _internalList.Count; i++) 
            {
                if (_internalList[i].Hashcode != hashCode) continue;

                _internalList.RemoveAt(i);
                return;
            }
        }

        public bool TryGetValue(T key, [MaybeNullWhen(false)] out U value)
        {
            return TryGetValue(key.GetHashCode(), out value);
        }
        private bool TryGetValue(int hashCode, [MaybeNullWhen(false)] out U value)
        {
            for (int i = 0; i < Count; i++) {
                if (_internalList[i].Hashcode != hashCode) continue;

                value = _internalList[i].Value;
                return true;
            }

            value = default;
            return false;
        }

        private void SetValue(int hashCode, U value) 
        {
            for (int i = 0; i < Count; i++) {
                if (_internalList[i].Hashcode != hashCode) continue;

                _internalList[i] = new (_internalList[i].Key, value, hashCode);
            }
        }

        public IEnumerator<KeyValueEntry<T, U>> GetEnumerator() 
        {
            return _internalList.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }
    }
}
