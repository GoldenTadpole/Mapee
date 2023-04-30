namespace CommonUtilities.Pool
{
    public class ExpandableObjectPool<TObject> : IResettablePool<TObject> 
    {
        public IList<TObject> Pool { get; set; }
        public int Index { get; set; }
        public Func<TObject> ObjectCreator { get; set; }

        public ExpandableObjectPool(int count, Func<TObject> objectCreator)
        {
            Pool = new List<TObject>(count);
            ObjectCreator = objectCreator;
            Index = 0;
        }

        public TObject Provide() 
        {
            if (Index >= Pool.Count) Pool.Add(ObjectCreator());
            return Pool[Index++];
        }
        public void Reset() 
        {
            Index = 0;
        }
    }
    public class ExpandableObjectPool<TInput, TObject> : IResettablePool<TInput, TObject> 
    {
        public IList<TObject> Pool { get; set; }
        public int Index { get; set; }
        public Func<TInput, TObject> ObjectCreator { get; set; }

        public ExpandableObjectPool(int count, Func<TInput, TObject> objectCreator)
        {
            Pool = new List<TObject>(count);
            ObjectCreator = objectCreator;
            Index = 0;
        }

        public TObject Provide(TInput input) 
        {
            if (Index >= Pool.Count) Pool.Add(ObjectCreator(input));
            return Pool[Index++];
        }
        public void Reset() 
        {
            Index = 0;
        }
    }
}
