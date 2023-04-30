using System.Collections;

namespace CommonUtilities.Collections.Synchronized
{
    public abstract class SynchronizedCollection<T> : ICollection<T>
    {
        protected ICollection<T> Collection { get; }
        protected object Root { get; }

        public int Count
        {
            get
            {
                lock (Root)
                {
                    return Collection.Count;
                }
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return Collection.IsReadOnly;
            }
        }

        public SynchronizedCollection(ICollection<T> collection)
        {
            Collection = collection;
            Root = ((ICollection)collection).SyncRoot;
        }
        public SynchronizedCollection(ICollection<T> collection, object root)
        {
            Collection = collection;
            Root = root;
        }

        public void Add(T item)
        {
            lock (Root)
            {
                Collection.Add(item);
            }
        }
        public bool Contains(T item)
        {
            lock (Root)
            {
                return Collection.Contains(item);
            }
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (Root)
            {
                Collection.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(T item)
        {
            lock (Root)
            {
                return Collection.Remove(item);
            }
        }
        public void Clear()
        {
            lock (Root)
            {
                Collection.Clear();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (Root)
            {
                return Collection.GetEnumerator();
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            lock (Root)
            {
                return Collection.GetEnumerator();
            }
        }
    }
}
