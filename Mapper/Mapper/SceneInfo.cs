using System.IO;
using WorldEditor;

namespace Mapper
{
    public struct SceneInfo
    {
        public Level Level { get; set; }
        public Dimension Dimension { get; set; }
        public IDictionary<Coords, RegionFile> RegionFiles { get; set; }

        public SceneInfo(Level level, Dimension dimension)
        {
            Level = level;
            Dimension = dimension;
            RegionFiles = new Dictionary<Coords, RegionFile>();

            string regionDirectory = $"{Level.Directory}\\{GetRegionFolder()}";
            IEnumerable<string> files = GetFiles(regionDirectory);
            ConvertFilesToRegions(files, RegionFiles);
        }

        public string? GetRegionFolder()
        {
            if (Dimension.Namespace.StartsWith("minecraft")) 
            {
                if (Dimension == Dimension.Overworld) return "region";
                if (Dimension == Dimension.Nether) return "DIM-1\\region";
                if (Dimension == Dimension.TheEnd) return "DIM1\\region";
                return "region";
            }

            string[] split = Dimension.Namespace.Split(':');
            string @namespace = split[0];
            string name = split[1];

            return $"dimensions\\{@namespace}\\{name}\\region";
        }
        private static IEnumerable<string> GetFiles(string directory)
        {
            if (!Directory.Exists(directory)) return Enumerable.Empty<string>();
            return Directory.GetFiles(directory);
        }
        private static void ConvertFilesToRegions(IEnumerable<string> files, IDictionary<Coords, RegionFile> output)
        {
            foreach (string file in files) 
            {
                if(!Parser.TryParseRegionName(Path.GetFileName(file), out int x, out int z)) continue;

                RegionFile regionFile = new RegionFile(new Coords(x, z), file);
                output.TryAdd(regionFile.Coords, regionFile);
            }
        }
    }
}
