using CommonUtilities.Data;

namespace WorldEditor
{
    public class DataPackReader : IObjectReader<string, DataPack?>
    {
        public virtual IObjectReader<CustomBiomeReadArgs, CustomBiome?> CustomBiomeReader { get; set; }

        public DataPackReader()
        {
            CustomBiomeReader = new CustomBiomeReader();
        }

        public virtual DataPack? Read(string path)
        {
            DataReader dataReader = new(path);

            string[]? namespaceFolders = dataReader.GetAllDirectories("data");
            if (namespaceFolders is null) return null;

            DataPack output = new();
            for (int i = 0; i < namespaceFolders.Length; i++)
            {
                ReadNamespace(namespaceFolders[i], dataReader, output);
            }

            return output;
        }
        protected virtual void ReadNamespace(string folder, IDataReader dataReader, DataPack output)
        {
            string biomeFolder = $"{folder}\\worldgen\\biome";

            string[]? biomes = dataReader.GetAllFiles(biomeFolder, "*.json", SearchOption.TopDirectoryOnly);
            ReadDimension(biomes, null, folder, dataReader, output);

            string[]? dimensions = dataReader.GetAllDirectories(biomeFolder);
            if (dimensions is null) return;

            foreach (string dimension in dimensions) 
            {
                biomes = dataReader.GetAllFiles(dimension, "*.json", SearchOption.TopDirectoryOnly);
                ReadDimension(biomes, Path.GetFileName(dimension), folder, dataReader, output);
            }
        }

        protected virtual void ReadDimension(string[]? biomes, string? dimension, string folder, IDataReader dataReader, DataPack output) 
        {
            if (biomes is null) return;

            foreach (string biomeFile in biomes)
            {
                byte[]? contents = dataReader.ReadFile(biomeFile);
                if (contents is null) continue;

                CustomBiomeReadArgs args = new()
                {
                    Namespace = Path.GetFileName(folder),
                    Dimension = dimension,
                    FileName = Path.GetFileName(biomeFile),
                    FileContents = contents
                };

                CustomBiome? biome = CustomBiomeReader.Read(args);
                if (biome is null) continue;

                output.CustomBiomes.Add(biome);
            }
        }
    }
}
