namespace WorldEditor
{
    public interface IChunk
    {
        int X { get; set; }
        int Z { get; set; }
        int LastModified { get; set; }
    }
}
