using System.Collections;

namespace CommonUtilities.Collections.Observable
{
    public abstract class ObservableCollection<T> : ICollection<T>
    {
        protected ICollection<T> BaseCollection { get; }

        public int Count => BaseCollection.Count;
        public bool IsReadOnly => BaseCollection.IsReadOnly;

        public event EventHandler? CollectionChanged;

        public ObservableCollection(ICollection<T> baseCollection) 
        {
            BaseCollection = baseCollection;
        }

        protected void InvokeEvent() 
        {
            CollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Add(T item)
        {
            BaseCollection.Add(item);
            InvokeEvent();
        }

        public bool Contains(T item)
        {
            return BaseCollection.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            BaseCollection.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            bool exists = BaseCollection.Remove(item);
            if (exists) InvokeEvent();

            return exists;
        }
        public void Clear()
        {
            int count = BaseCollection.Count;
            BaseCollection.Clear();

            if(count > 0) InvokeEvent();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return BaseCollection.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return BaseCollection.GetEnumerator();
        }
    }
}
