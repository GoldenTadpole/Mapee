namespace CommonUtilities.Collections.Synchronized
{
    public class SynchronizedList<T> : SynchronizedCollection<T>, IList<T>
    {
        protected IList<T> List { get; }

        public T this[int index]
        {
            get
            {
                lock (Root)
                {
                    return List[index];
                }
            }
            set
            {
                lock (Root)
                {
                    List[index] = value;
                }
            }
        }

        public SynchronizedList() : this(new List<T>()) { }
        public SynchronizedList(int capacity) : this(new List<T>(capacity)) { }
        public SynchronizedList(IList<T> list) : base(list)
        {
            List = list;
        }

        public int IndexOf(T item)
        {
            lock (Root)
            {
                return List.IndexOf(item);
            }
        }
        public void Insert(int index, T item)
        {
            lock (Root)
            {
                List.Insert(index, item);
            }
        }
        public void RemoveAt(int index)
        {
            lock (Root)
            {
                List.RemoveAt(index);
            }
        }
    }
}
