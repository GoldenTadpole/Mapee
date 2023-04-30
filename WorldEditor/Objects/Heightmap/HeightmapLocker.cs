namespace WorldEditor
{
    public class HeightmapLocker : Locker
    {
        public override int BitCount => 9;
        public override int UnlockedArrayLength => 16 * 16;
    }
}
