using System.Collections;

namespace CommonUtilities.Collections.Synchronized
{
    public class SynchronizedSet<T> : SynchronizedCollection<T>, ISet<T>
    {
        protected ISet<T> Set { get; }

        public SynchronizedSet() : this(new HashSet<T>()) { }
        public SynchronizedSet(int capacity) : this(new HashSet<T>(capacity)) { }
        public SynchronizedSet(ISet<T> set) : base(set, new object())
        {
            Set = set;
        }

        public new bool Add(T item)
        {
            lock (Root)
            {
                return Set.Add(item);
            }
        }
        void ICollection<T>.Add(T item)
        {
            lock (Root)
            {
                Set.Add(item);
            }
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            lock (Root)
            {
                Set.ExceptWith(other);
            }
        }
        public void IntersectWith(IEnumerable<T> other)
        {
            lock (Root)
            {
                Set.IntersectWith(other);
            }
        }
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            lock (Root)
            {
                Set.SymmetricExceptWith(other);
            }
        }
        public void UnionWith(IEnumerable<T> other)
        {
            lock (Root)
            {
                Set.UnionWith(other);
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            lock (Root)
            {
                return Set.IsProperSubsetOf(other);
            }
        }
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            lock (Root)
            {
                return Set.IsProperSupersetOf(other);
            }
        }
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            lock (Root)
            {
                return Set.IsSubsetOf(other);
            }
        }
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            lock (Root)
            {
                return Set.IsSupersetOf(other);
            }
        }
        public bool Overlaps(IEnumerable<T> other)
        {
            lock (Root)
            {
                return Set.Overlaps(other);
            }
        }
        public bool SetEquals(IEnumerable<T> other)
        {
            lock (Root)
            {
                return Set.SetEquals(other);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (Root)
            {
                return Set.GetEnumerator();
            }
        }
    }
}
