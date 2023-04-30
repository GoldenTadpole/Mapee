using AssetSystem.Block;
using AssetSystem;
using CommonUtilities.Data;

namespace Mapper
{
    public class TokenAsset<TOutput> where TOutput : struct
    {
        public BlockAssetArgs Args { get; set; }
        public IDataReader? Data { get; set; }
        public ITokenizedBlockAsset<TOutput>? Cloneable { get; set; }
        public IAssetReader<BlockReadArgs, TOutput?>? Reader { get; set; }

        private const string USE_DEFAULT_MODIFIER = "*UseDefault";

        public ITokenizedBlockAsset<TOutput>? Read()
        {
            ITokenizedBlockAsset<TOutput>? output = null;

            if (Args.Source is not null)
            {
                if (Args.Source == USE_DEFAULT_MODIFIER)
                {
                    if (Cloneable is not null) output = Cloneable.Clone();
                }
                else
                {
                    if (Data is not null && Reader is not null) output = new BlockAssetReader<TOutput>(Reader).Read(new AssetArgs(Data, Args.Source));
                }
            }

            output ??= TokenizedBlockAsset<TOutput>.Empty;
            SetDefaultBlockValue(output);

            return output;
        }
        private void SetDefaultBlockValue<TInput>(IAsset<TInput, TOutput>? output)
        {
            if (output is not null && Reader is not null && Args.DefaultOutputExists)
            {
                output.DefaultOutput = Reader.Read(new BlockReadArgs(Args.DefaultOutput, string.Empty, null)) ?? default;
            }
        }
    }
}
