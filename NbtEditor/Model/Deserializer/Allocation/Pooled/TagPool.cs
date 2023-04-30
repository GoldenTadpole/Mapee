using CommonUtilities.Pool;

namespace NbtEditor
{
    public class TagPool<TInput, TObject> : IPool<TInput, TObject>
    {
        public IPool<TInput, TObject> Pool { get; set; }
        public Func<TInput, TObject, TObject> Function { get; set; }

        public TagPool(IPool<TInput, TObject> pool, Func<TInput, TObject, TObject> function)
        {
            Pool = pool;
            Function = function;
        }

        public TObject Provide(TInput input)
        {
            return Function(input, Pool.Provide(input));
        }
    }
}
