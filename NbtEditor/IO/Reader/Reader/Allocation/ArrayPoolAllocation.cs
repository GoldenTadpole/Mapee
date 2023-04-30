using CommonUtilities.Pool;

namespace NbtEditor 
{
    public class ArrayPoolAllocation 
    {
        public virtual IPool<int, sbyte[]> SBytePool { get; set; }
        public virtual IPool<int, int[]> Int32Pool { get; set; }
        public virtual IPool<int, long[]> Int64Pool { get; set; }

        public static ArrayPoolAllocation CreateDefault() 
        {
            return new ArrayPoolAllocation() 
            {
                SBytePool = new EmptyPool<int, sbyte[]>(length => new sbyte[length]),
                Int32Pool = new EmptyPool<int, int[]>(length => new int[length]),
                Int64Pool = new EmptyPool<int, long[]>(length => new long[length]),
            };
        }
    }
}
