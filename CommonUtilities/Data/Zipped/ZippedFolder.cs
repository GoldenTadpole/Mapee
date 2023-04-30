using System.IO.Compression;

namespace CommonUtilities.Data
{
    public class ZippedFolder {
        public ZippedFolder? Parent { get; private set; }

        public string FullName { get; }

        public IList<ZippedFolder> Folders { get; }
        public IList<ZippedFile> Files { get; }

        public ZippedFolder(ZippedFolder? parent, string fullName)
        {
            Parent = parent;
            FullName = fullName;

            Folders = new List<ZippedFolder>();
            Files = new List<ZippedFile>();
        }

        public ZippedFolder? GetChildFolder(string[] path, int index = 0)
        {
            foreach (ZippedFolder child in Folders)
            {
                if (Path.GetFileName(child.FullName) != path[index]) continue;

                if (index == path.Length - 1) return child;
                return child.GetChildFolder(path, index + 1);
            }

            return null;
        }

        public static ZippedFolder FromFile(string file)
        {
            ZippedFolder output = new ZippedFolder(null, string.Empty);

            Dictionary<string, ZippedFolder> parents = new Dictionary<string, ZippedFolder>(20)
            {
                { output.FullName, output }
            };

            using (System.IO.Compression.ZipArchive zip = ZipFile.Open(file, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    if (entry.FullName.EndsWith("/")) continue;
                    
                    ZippedFolder folder = CreateChainParents(parents, entry.FullName);
                    folder.Files.Add(ZippedFile.FromZipArchiveEntry(entry));
                }
            }

            foreach (KeyValuePair<string, ZippedFolder> pair in parents)
            {
                ZippedFolder folder = pair.Value;
                if (string.IsNullOrEmpty(folder.FullName)) continue;

                string? directoaryName = Path.GetDirectoryName(folder.FullName);
                if (directoaryName is null) continue;

                ZippedFolder parentFolder = parents[directoaryName];

                folder.Parent = parentFolder;
                parentFolder.Folders.Add(folder);
            }

            return output;
        }
        private static ZippedFolder CreateChainParents(IDictionary<string, ZippedFolder> parents, string currentPath)
        {
            ZippedFolder? output = null;

            string parent = currentPath;
            while (true) 
            {
                string? directoaryName = Path.GetDirectoryName(parent);
                if (directoaryName is null) continue;

                parent = directoaryName;

                if (!parents.ContainsKey(parent))
                {
                    ZippedFolder folder = new ZippedFolder(null, parent);
                    parents.Add(folder.FullName, folder);

                    output ??= folder;
                    if (string.IsNullOrEmpty(parent)) break;
                }
                else
                {
                    output ??= parents[parent];
                    break;
                }
            }

            return output;
        }
    }
}
