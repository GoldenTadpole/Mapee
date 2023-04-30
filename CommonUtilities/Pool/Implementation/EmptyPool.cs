namespace CommonUtilities.Pool
{
    public class EmptyPool<TObject> : IPool<TObject>
    {
        public Func<TObject> ObjectCreator { get; set; }

        public EmptyPool(Func<TObject> objectCreator)
        {
            ObjectCreator = objectCreator;
        }

        public TObject Provide()
        {
            return ObjectCreator();
        }
    }

    public class EmptyPool<TInput, TObject> : IPool<TInput, TObject>
    {
        public Func<TInput, TObject> ObjectCreator { get; set; }

        public EmptyPool(Func<TInput, TObject> objectCreator) 
        {
            ObjectCreator = objectCreator;
        }

        public TObject Provide(TInput input) 
        {
            return ObjectCreator(input);
        }
    }
}
