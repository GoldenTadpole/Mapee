namespace CommonUtilities.Pool
{
    public class ThreadedPool<TInput, TOutput> : IPool<TOutput>
    {
        public IPool<TInput, TOutput> Pool { get; set; }
        public string DataSlotName { get; set; }

        public ThreadedPool(string dataSlotName, IPool<TInput, TOutput> pool)
        {
            Pool = pool;
            DataSlotName = dataSlotName;
        }

        public TOutput Provide()
        {
            TInput? data = (TInput?)Thread.GetData(Thread.GetNamedDataSlot(DataSlotName));
            if (data is null) throw new Exception();

            TInput input = data;
            return Pool.Provide(input);
        }
    }
}
