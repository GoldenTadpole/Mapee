using CommonUtilities.Pool;

namespace NbtEditor
{
    public class ArrayTagAllocation
    {
        public virtual IPool<sbyte[], ArrayTag> PreAllocatedSByteArrays { get; set; }
        public virtual IPool<int[], ArrayTag> PreAllocatedInt32Arrays { get; set; }
        public virtual IPool<long[], ArrayTag> PreAllocatedInt64Arrays { get; set; }

        public static ArrayTagAllocation CreateDefault() 
        {
            return new ArrayTagAllocation() 
            { 
                PreAllocatedSByteArrays = new EmptyPool<sbyte[], ArrayTag>(array => new ArrayTag(array)),
                PreAllocatedInt32Arrays = new EmptyPool<int[], ArrayTag>(array => new ArrayTag(array)),
                PreAllocatedInt64Arrays = new EmptyPool<long[], ArrayTag>(array => new ArrayTag(array))
            };
        }
    }
}
