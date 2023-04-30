using CommonUtilities.Data;

namespace AssetSystem
{
    public readonly struct AssetArgs
    {
        public IDataReader Reader { get; }
        public string[]? Files { get; }

        public AssetArgs(IDataReader reader, string directory) 
        {
            Reader = reader;
            Files = Reader.GetAllFiles(directory, "*.json");
        }
    }
}
