namespace WorldEditor
{
    public interface IIdTranslator
    {
        Block Translate(byte blockState, byte data);
    }
}
