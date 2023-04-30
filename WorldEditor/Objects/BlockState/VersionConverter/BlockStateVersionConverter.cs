namespace WorldEditor
{
    public class BlockStateVersionConverter : VersionConverter
    {
        protected override void InitializeConverters()
        {
            Converters.Add(new AlphaBlockStateInstanceConverter());
            Converters.Add(new AnvilBlockStateInstanceConverter()
            {
                From = new VersionRange(Version.Post_1_1, Version.Snapshot_17w46a),
                To = new VersionRange(Version.Snapshot_17w47a, Version.Snapshot_18w10a),
                IdTranslator = IdTranslator.FromFile($"Translators\\Block\\IdTranslator\\{Version.Snapshot_17w47a}.json")
            });
            Converters.Add(new AnvilBlockStateInstanceConverter()
            {
                From = new VersionRange(Version.Post_1_1, Version.Snapshot_17w46a),
                To = new VersionRange(Version.Snapshot_17w47a, Version.Rel_1_13),
                IdTranslator = IdTranslator.FromFile($"Translators\\Block\\IdTranslator\\{Version.Rel_1_13}.json")
            });

            Release[] releases = new Release[] {
                new Release("1.13", new[]{ Version.Rel_1_13_1 }),
                new Release("1.14", new[]{ Version.Rel_1_14 }),
                new Release("1.15", new[]{ Version.Rel_1_15 }),
                new Release("1.16", new[]{ Version.Rel_1_16, Version.Rel_1_16_2 }),
                new Release("1.17", new[]{ Version.Rel_1_17 }),
                new Release("1.19", new[]{ Version.Rel_1_19 }),
            };

            Func<VersionRange, VersionRange, string, IInstanceConverter<IObject>> converterCreator = (from, to, path) =>
            {
                return new BlockStateRenamerInstanceConverter()
                {
                    From = from,
                    To = to,
                    Renamer = BlockRenamer.FromFile(path)
                };
            };

            LoadVersionConverters("Translators\\Block\\NamespaceRenamer", releases, Version.Rel_1_13, Version.Snapshot_17w47a, converterCreator);
        }
    }
}
