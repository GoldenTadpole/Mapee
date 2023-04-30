using System.Collections;

namespace CommonUtilities.Collections.Simple
{
    public class SimpleList<T> : IList<T>, ICollection where T: notnull
    {
        public T[] InternalArray => _internalArray;
        private T[] _internalArray;

        private ushort _count;

        public int Count => _count;
        public bool IsReadOnly => false;

        public bool IsSynchronized => false;
        public object SyncRoot => new object();

        public T this[int index] 
        { 
            get => _internalArray[index];
            set => _internalArray[index] = value;
        }

        public SimpleList(int count = 2)
        { 
            _internalArray = new T[count];
            _count = 0;
        }
        public SimpleList(T[] internalArray)
        {
            _internalArray = internalArray;
            _count = 0;
        }

        public void Add(T value)
        {
            if (_count == _internalArray.Length) IncreaseArray();

            _internalArray[_count++] = value;
        }
        public void Insert(int index, T item) 
        {
            if (++_count > _internalArray.Length) IncreaseArray();

            for (int i = _count - 1; i > index; i--)
            { 
                _internalArray[i] = _internalArray[i - 1];
            }

            _internalArray[index] = item;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index < 0) return false;

            RemoveAt(index);
            return true;
        }
        public void RemoveAt(int index)
        {
            for (int i = index; i < _count - 1; i++) 
            {
                _internalArray[i] = _internalArray[i + 1];
            }

            Array.Clear(_internalArray, _count - 1, 1);

            _count--;
        }
        public void Clear()
        {
            Array.Clear(_internalArray, 0, _count);
            _count = 0;
        }
        public void ClearInternalArray()
        {
            _count = 0;
            _internalArray = new T[2];
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _count; i++) {
                if (_internalArray[i].Equals(item)) return i;
            }

            return -1;
        }
        public bool Contains(T item)
        {
            return IndexOf(item) > -1;
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            _internalArray.CopyTo(array, arrayIndex);
        }
        public void CopyTo(Array array, int index)
        {
            _internalArray.CopyTo(array, 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++) {
                yield return _internalArray[i];
            }
        }

        private void IncreaseArray()
        {
            Array.Resize(ref _internalArray, Math.Max(_internalArray.Length * 2, 2));
        }
    }
}
