namespace WorldEditor
{
    public abstract class Locker
    {
        public IBlockStateWriter? Writer { get; set; }
        public IBlockStateReader? Reader { get; set; }

        public abstract int BitCount { get; }
        public abstract int UnlockedArrayLength { get; }

        public virtual long[] Lock(short[] unlockedArray)
        {
            if (Writer is null) throw new ArgumentNullException(nameof(Writer));
            return Writer.Write(unlockedArray, BitCount);
        }
        public virtual void Unlock(long[] lockedArray, short[] outputArray)
        {
            if(Reader is null) throw new Exception(nameof(Reader));
            Reader.Read(lockedArray, BitCount, outputArray);
        }
    }
}
