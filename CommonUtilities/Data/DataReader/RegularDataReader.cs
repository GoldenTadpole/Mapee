namespace CommonUtilities.Data
{
    public class RegularDataReader : IDataReader
    {
        public string BasePath { get; set; }

        public RegularDataReader(string basePath) 
        {
            BasePath = basePath;
        }

        public string[]? GetAllDirectories(string directory, string filter = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            directory = GetAbsolutePath(directory);
            if (!File.Exists(directory)) return null;

            return ConvertToRelative(Directory.GetDirectories(directory, filter, searchOption));
        }
        public string[]? GetAllFiles(string directory, string filter = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            directory = GetAbsolutePath(directory);
            if (!Directory.Exists(directory)) return null;

            return ConvertToRelative(Directory.GetFiles(directory, filter, searchOption));
        }

        public byte[]? ReadFile(string path)
        {
            path = GetAbsolutePath(path);
            if(!File.Exists(path)) return null;

            return File.ReadAllBytes(path);
        }

        public IDataReader? CreateChild(string directory)
        {
            directory = GetAbsolutePath(directory);
            if (!Directory.Exists(directory)) return null;

            return new RegularDataReader(directory);
        }

        private string GetAbsolutePath(string relative) 
        {
            if (string.IsNullOrEmpty(relative)) return BasePath;
            return $"{BasePath}\\{relative}";
        }
        private string[]? ConvertToRelative(string[]? absolute) 
        {
            if (absolute is null) return null;

            for (int i = 0; i < absolute.Length; i++) 
            {
                absolute[i] = Path.GetRelativePath(BasePath, absolute[i]);
            }

            return absolute;
        }
    }
}
