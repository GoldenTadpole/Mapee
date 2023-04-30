namespace AssetSystem
{
    public interface IAssetReader<in TInput, out TOuput>
    {
        TOuput Read(TInput input);
    }
    public interface IAssetReader<in TInput>
    {
        void Read(TInput input);
    }
}
