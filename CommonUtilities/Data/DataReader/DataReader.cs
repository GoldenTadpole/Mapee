namespace CommonUtilities.Data
{
    public class DataReader : IDataReader
    {
        public IDataReader BaseDataReader { get; }

        public DataReader(string path) 
        {
            if (File.Exists(path))
            {
                BaseDataReader = new ZippedDataReader(ZippedFolder.FromFile(path));
            }
            else 
            {
                BaseDataReader = new RegularDataReader(path);
            }
        }

        public string[]? GetAllDirectories(string directory, string filter = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            return BaseDataReader.GetAllDirectories(directory, filter, searchOption);
        }
        public string[]? GetAllFiles(string directory, string filter = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            return BaseDataReader.GetAllFiles(directory, filter, searchOption);
        }

        public byte[]? ReadFile(string path)
        {
            return BaseDataReader.ReadFile(path);
        }

        public IDataReader? CreateChild(string directory)
        {
            return BaseDataReader.CreateChild(directory);
        }
    }
}
