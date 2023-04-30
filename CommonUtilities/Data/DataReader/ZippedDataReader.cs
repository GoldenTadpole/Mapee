using System.Text.RegularExpressions;

namespace CommonUtilities.Data
{
    public class ZippedDataReader : IDataReader
    {
        public ZippedFolder BaseZipFolder { get; }
        public string BasePath { get; set; }

        private readonly IList<ZippedFolder> _directories;
        private readonly IDictionary<string, ZippedFile> _files;

        public ZippedDataReader(ZippedFolder baseZipFolder, string basePath = "")
        {
            BaseZipFolder = baseZipFolder;
            BasePath = basePath;

            _directories = new List<ZippedFolder>();
            AddToDirectories(baseZipFolder, _directories);

            _files = new Dictionary<string, ZippedFile>();
            AddToFiles(BaseZipFolder, _files);
        }

        private static void AddToDirectories(ZippedFolder folder, IList<ZippedFolder> directories) 
        {
            directories.Add(folder);

            foreach (ZippedFolder childFolder in folder.Folders)
            {
                AddToDirectories(childFolder, directories);
            }
        }
        private static void AddToFiles(ZippedFolder folder, IDictionary<string, ZippedFile> files)
        {
            foreach (ZippedFolder childFolder in folder.Folders)
            {
                AddToFiles(childFolder, files);
            }

            foreach (ZippedFile childFile in folder.Files)
            {
                files.Add(childFile.FullName.Replace("/", "\\"), childFile);
            }
        }

        public string[]? GetAllDirectories(string directory, string filter = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            directory = GetAbsolutePath(directory);
            Regex regex = CreateRegex(filter);

            List<string> output = new();
            foreach (ZippedFolder folder in _directories)
            {
                string path = folder.FullName.Replace("/", "\\");
                if (!IsSubPath(directory, path)) continue;
                if (searchOption == SearchOption.TopDirectoryOnly && Path.GetDirectoryName(path) != directory) continue;

                string name = Path.GetFileName(path);
                if (regex.IsMatch(name)) output.Add(path);
            }

            return ConvertToRelative(output.ToArray());
        }
        public string[]? GetAllFiles(string directory, string filter = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            directory = GetAbsolutePath(directory);
            Regex regex = CreateRegex(filter);

            List<string> output = new();
            foreach (KeyValuePair<string, ZippedFile> pair in _files) 
            {
                if (!IsSubPath(directory, pair.Key)) continue;
                if (searchOption == SearchOption.TopDirectoryOnly && Path.GetDirectoryName(pair.Key) != directory) continue;

                string name = Path.GetFileName(pair.Key);
                if (regex.IsMatch(name)) output.Add(pair.Key);
            }

            return ConvertToRelative(output.ToArray());
        }
        
        public byte[]? ReadFile(string path)
        {
            if (!_files.TryGetValue(GetAbsolutePath(path), out ZippedFile? file) || file is null) return null;
            return file.Content;
        }

        public IDataReader? CreateChild(string directory)
        {
            string abs = GetAbsolutePath(directory);

            foreach (ZippedFolder folder in _directories) 
            {
                string path = folder.FullName.Replace("/", "\\");
                if (path != abs) continue;

                return new ZippedDataReader(folder, directory);
            }

            return null;
        }

        private static Regex CreateRegex(string filter) 
        {
            string fileNamePattern = filter;
            string regexPattern = "^" + Regex.Escape(fileNamePattern)
                .Replace(@"\*", ".*")
                .Replace(@"\?", ".")
                + "$";

            return new(regexPattern);
        }

        private string GetAbsolutePath(string relative)
        {
            if (string.IsNullOrEmpty(BasePath)) return relative;
            return $"{BasePath}\\{relative}";
        }
        private static bool IsSubPath(string basePath, string potentionSubPath) 
        {
            string parentUri = basePath;
            string? childUri = Path.GetDirectoryName(potentionSubPath);

            while (childUri is not null)
            {
                if (childUri == parentUri) return true;   
                childUri = Path.GetDirectoryName(childUri);
            }

            return false;
        }
        private string[]? ConvertToRelative(string[]? absolute)
        {
            if (absolute is null) return null;
            if (string.IsNullOrEmpty(BasePath)) return absolute;

            for (int i = 0; i < absolute.Length; i++)
            {
                absolute[i] = Path.GetRelativePath(BasePath, absolute[i]);
            }

            return absolute;
        }
    }
}
