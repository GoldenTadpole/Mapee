namespace AssetSystem
{
    public interface IAsset<in TInput, TOutput> where TOutput : struct
    {
        TOutput DefaultOutput { get; set; }

        AssetReturn<TOutput> Provide(TInput input);
        IAsset<TInput, TOutput> Clone();
    }
}
