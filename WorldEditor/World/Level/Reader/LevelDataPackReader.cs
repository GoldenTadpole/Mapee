using NbtEditor;

namespace WorldEditor
{
    public class LevelDataPackReader : IObjectReader<LevelArgs, IList<string>>
    {
        public string IgnoreDataPack { get; } = "vanilla";

        public IList<string> Read(LevelArgs input)
        {
            if (!input.Data.TryGetChild(out Tag listTag, "DataPacks", "Enabled")) return Enumerable.Empty<string>().ToList();
            if (listTag is not ListTag enabledDataPacks) return Enumerable.Empty<string>().ToList();

            List<string> output = new();
            foreach (string file in enabledDataPacks)
            {
                if (file == IgnoreDataPack) continue;
                output.Add(file);
            }

            return output;
        }
    }
}
