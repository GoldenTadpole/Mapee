namespace WorldEditor
{
    public abstract class VersionConverter : IVersionConverter
    {
        public IList<IInstanceConverter<IObject?>> Converters { get; set; }

        public VersionConverter()
        {
            Converters = new List<IInstanceConverter<IObject?>>();
            InitializeConverters();
        }

        public IObject? Convert(IObject input, Version from, Version to, UsageIntent intent)
        {
            IObject? output = input;

            foreach (IInstanceConverter<IObject?> converter in GetConverters(from, to))
            {
                if (output is null) return null;
                output = converter.Convert(output, intent);
            }

            return output;
        }
        protected virtual IEnumerable<IInstanceConverter<IObject?>> GetConverters(Version from, Version to)
        {
            List<IInstanceConverter<IObject?>> output = new();

            for (int i = Converters.Count - 1; i >= 0; i--)
            {
                IInstanceConverter<IObject?> converter = Converters[i];
                if (!converter.To.IsInRange(to)) continue;
                if (from > converter.From.End) continue;

                output.Add(converter);

                if (converter.From.IsInRange(from)) break;
                to = converter.From.Start;
            }

            output.Reverse();
            return output;
        }

        protected abstract void InitializeConverters();

        protected void LoadVersionConverters(string folder, Release[] releases, Version releaseStart, Version snapshotStart, Func<VersionRange, VersionRange, string, IInstanceConverter<IObject>> converterCreator)
        {
            Version[][] snapshots = new Version[releases.Length][];
            for (int i = 0; i < snapshots.Length; i++)
            {
                snapshots[i] = GetVersions($"{folder}\\Snapshot\\{releases[i].FolderName}");
            }

            VersionRange? lastTo = null;
            for (int i = 0; i < releases.Length; i++)
            {
                Release release = releases[i];

                Version sStart, sEnd;
                if (i == 0) sStart = snapshotStart;
                else sStart = snapshots[i - 1].Last();

                if (i >= releases.Length - 1) sEnd = Version.Newest;
                else sEnd = snapshots[i + 1][0].Prev();

                LoadSnapshotConverters($"{folder}\\Snapshot\\{release.FolderName}", sStart, sEnd, converterCreator);

                for (int j = 0; j < release.Releases.Length; j++)
                {
                    VersionRange from, to;
                    if (!lastTo.HasValue) from = new VersionRange(releaseStart);
                    else from = lastTo.Value;

                    bool jIsLast = j >= release.Releases.Length - 1;

                    if (i >= releases.Length - 1 && jIsLast) to = new VersionRange(release.Releases[j], Version.Newest);
                    else
                    {
                        if (jIsLast) to = new VersionRange(release.Releases[j], releases[i + 1].Releases[0].Prev());
                        else to = new VersionRange(release.Releases[j], release.Releases[j + 1].Prev());
                    }

                    lastTo = to;

                    Converters.Add(converterCreator(from, to, $"{folder}\\Release\\{release.Releases[j]}.json"));
                }
            }
        }
        protected void LoadSnapshotConverters(string folder, Version start, Version end, Func<VersionRange, VersionRange, string, IInstanceConverter<IObject>> converterCreator)
        {
            Version[] versions = GetVersions(folder);

            VersionRange lastTo = new();
            for (int i = 0; i < versions.Length; i++)
            {
                Version version = versions[i], prev = version.Prev();

                VersionRange from, to;
                if (i == 0) from = new VersionRange(start, prev);
                else from = lastTo;

                if (i >= versions.Length - 1) to = new VersionRange(version, end);
                else to = new VersionRange(version, versions[i + 1].Prev());

                lastTo = to;
                Converters.Add(converterCreator(from, to, $"{folder}\\{version}.json"));
            }
        }
        private static Version[] GetVersions(string folder)
        {
            string[] files = Directory.GetFiles(folder);

            Version[] versions = new Version[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                if (Enum.TryParse(Path.GetFileNameWithoutExtension(files[i]), out Version version))
                {
                    versions[i] = version;
                }
            }
            return versions.OrderBy(x => x).ToArray();
        }

        protected readonly struct Release
        {
            public string FolderName { get; }
            public Version[] Releases { get; }

            public Release(string folderName, Version[] releases)
            {
                FolderName = folderName;
                Releases = releases;
            }
        }
    }
}
