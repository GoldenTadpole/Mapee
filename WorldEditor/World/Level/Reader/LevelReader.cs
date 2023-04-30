using NbtEditor;

namespace WorldEditor
{
    public class LevelReader : IObjectReader<string, Level?>
    {
        public IKnownTypeCompression GZipCompression { get; set; }

        public IObjectReader<LevelArgs, LevelVersion> VersionReader { get; set; }
        public IObjectReader<LevelArgs, Player> PlayerReader { get; set; }
        public IObjectReader<LevelArgs, WorldGen> WorldGenReader { get; set; }
        public IObjectReader<LevelArgs, IList<string>> DataPackReader { get; set; }

        private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public LevelReader()
        {
            GZipCompression = new GZipCompression();
            VersionReader = new LevelVersionReader();
            PlayerReader = new PlayerReader();
            WorldGenReader = new WorldGenReader();
            DataPackReader = new LevelDataPackReader();
        }

        public Level? Read(string file)
        {
            ArraySlice<byte> decompressed = Decompress(file);

            Tag levelTag = Tag.FromBytes(decompressed.Array, decompressed.Position);
            if (levelTag is not CompoundTag compoundLevel) return null;
            if (!compoundLevel.TryGetChild("Data", out Tag dataChild) || dataChild is not CompoundTag data) return null;

            Level output = new Level()
            {
                Tag = compoundLevel,
                Directory = Path.GetDirectoryName(file)
            };

            LevelArgs args = new LevelArgs(data, output);

            output.Version = VersionReader.Read(args);
            output.Player = PlayerReader.Read(args);
            output.WorldGen = WorldGenReader.Read(args);
            output.DataPacks = DataPackReader.Read(args);

            ReadRest(args);

            return output;
        }
        private ArraySlice<byte> Decompress(string file)
        {
            ArraySlice<byte> input = new ArraySlice<byte>(File.ReadAllBytes(file));
            ArraySlice<byte> output = new ArraySlice<byte>(new byte[1024 * 1024]);

            GZipCompression.Compress(input, output);

            return output;
        }

        private static void ReadRest(LevelArgs args)
        {
            if (args.Data.TryGetChild("LevelName", out Tag levelNameTag) && levelNameTag.Id == TagId.String) 
            {
                args.Level.WorldName = levelNameTag;
            }

            if (args.Data.TryGetChild("GameType", out Tag gameTypeTag) && gameTypeTag.Id == TagId.Int32) 
            {
                args.Level.GameType = (GameType)(int) gameTypeTag;
            }

            if (args.Data.TryGetChild("allowCommands", out Tag allowCommandsTag) && allowCommandsTag.Id == TagId.SignedByte) 
            {
                args.Level.AllowCommands = ((sbyte)allowCommandsTag) == 1;
            }

            if (args.Data.TryGetChild("hardcore", out Tag hardcoreTag) && hardcoreTag.Id == TagId.SignedByte) 
            {
                args.Level.IsHardcode = ((sbyte)hardcoreTag) == 1;
            }

            if (args.Data.TryGetChild("LastPlayed", out Tag lastPlayedTag) && lastPlayedTag.Id == TagId.Int64) 
            {
                args.Level.LastPlayed = UnixEpoch.AddMilliseconds((long)lastPlayedTag);
            }
        }
    }
}
