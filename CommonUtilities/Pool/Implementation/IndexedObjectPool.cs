namespace CommonUtilities.Pool
{
    public class IndexedObjectPool<TObject> : IPool<int, TObject>
    {
        public TObject[] Pool { get; set; }

        public IndexedObjectPool(int length, Func<int, TObject> poolCreator)
        {
            Pool = new TObject[length];

            for (int i = 0; i < Pool.Length; i++)
            {
                Pool[i] = poolCreator(i);
            }
        }

        public TObject Provide(int index)
        {
            return Pool[index];
        }
    }
}
