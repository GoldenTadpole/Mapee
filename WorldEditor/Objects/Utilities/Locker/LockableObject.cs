namespace WorldEditor
{
    public abstract class LockableObject
    {
        public Locker? Locker { get; set; }

        protected abstract long[] LockedArray { get; set; }

        public virtual void Lock(short[] unlockedArray)
        {
            if(Locker is null) throw new ArgumentNullException(nameof(Locker));
            LockedArray = Locker.Lock(unlockedArray);
        }
        public virtual void Unlock(short[] outputArray)
        {
            if (Locker is null) throw new ArgumentNullException(nameof(Locker));
            Locker.Unlock(LockedArray, outputArray);
        }
    }
}
