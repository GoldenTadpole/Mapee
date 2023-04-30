namespace CommonUtilities.Pool
{
    public class ExpandableIndexedPool<TObject> : IPool<int, TObject>
    {
        public IList<TObject> Pool { get; set; }
        public Func<int, TObject> ObjectCreator { get; set; }

        private object _lock = new object();

        public ExpandableIndexedPool(int capacity, Func<int, TObject> objectCreator)
        {
            Pool = new List<TObject>(capacity);
            ObjectCreator = objectCreator;
        }

        public TObject Provide(int index)
        {
            if (index >= Pool.Count) IncreaseRange(index - Pool.Count + 1);

            return Pool[index];
        }

        private void IncreaseRange(int count)
        {
            lock (_lock)
            {
                for (int i = 0; i < count; i++)
                {
                    Pool.Add(ObjectCreator(Pool.Count - 1));
                }
            }
        }
    }
}
