namespace CommonUtilities.Data
{
    public interface IDataReader
    {
        string[]? GetAllDirectories(string directory, string filter = "*", SearchOption searchOption = SearchOption.AllDirectories);
        string[]? GetAllFiles(string directory, string filter = "*", SearchOption searchOption = SearchOption.AllDirectories);

        byte[]? ReadFile(string path);

        IDataReader? CreateChild(string directory);
    }
}
