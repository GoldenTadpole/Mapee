namespace AssetSystem.Biome
{
    public class BiomeAsset<TOutput> : IBiomeAsset<TOutput> where TOutput: struct
    {
        public IReadOnlyDictionary<int, PropertyMatcher<TOutput>> BiomeBlocks { get; }
        private readonly IDictionary<int, PropertyMatcher<TOutput>> _biomeBlocks;

        private ISet<int> _blockCache = new HashSet<int>();

        public string? DefaultBiome {
            get => _defaultBiome;
            set {
                if (value is null) return;

                _defaultBiome = value;
                _defaultBiomeHash = value.GetHashCode();
            }
        }
        private string? _defaultBiome;
        private int _defaultBiomeHash;

        public TOutput DefaultOutput { get; set; } = default;

        public BiomeAsset() 
        {
            _biomeBlocks = new Dictionary<int, PropertyMatcher<TOutput>>();
            BiomeBlocks = _biomeBlocks.AsReadOnly();
        }

        public AssetReturn<TOutput> Provide(BiomeBlock input)
        {
            int blockHash = input.Block.Name.GetHashCode();
            if (!_blockCache.Contains(blockHash)) 
            {
                return new AssetReturn<TOutput>(DefaultOutput, false);
            }
            
            int inputHash = BiomeBlock.GetHashCode(blockHash, input.Biome.GetHashCode());

            if (TryGetOutput(input, inputHash, out TOutput output))
            {
                return output;
            }

            if (DefaultBiome is not null)
            {
                BiomeBlock defaultInput = new BiomeBlock(input.Block, DefaultBiome);
                int defaultHash = BiomeBlock.GetHashCode(blockHash, _defaultBiomeHash);

                if (TryGetOutput(defaultInput, defaultHash, out output)) return output;
            }

            return new AssetReturn<TOutput>(DefaultOutput, false);
        }

        private bool TryGetOutput(BiomeBlock input, int hashCode, out TOutput output)
        {
            if (_biomeBlocks.TryGetValue(hashCode, out PropertyMatcher<TOutput> evaluator))
            {
                PropertyValueProvider propertyValueProvider = PropertyValueProviderUtilities.CreateGetter(input.Block.Properties);
                if (evaluator.Match(propertyValueProvider))
                {
                    output = evaluator.Payload;
                    return true;
                }
            }
            
            output = default;
            return false;
        }

        public void Add(BiomeBlock key, PropertyMatcher<TOutput> evaluator) {
            _biomeBlocks.Add(key.GetHashCode(), evaluator);
            _blockCache.Add(key.Block.Name.GetHashCode());
        }

        IAsset<BiomeBlock, TOutput> IAsset<BiomeBlock, TOutput>.Clone()
        {
            return Clone();
        }
        public IBiomeAsset<TOutput> Clone()
        {
            BiomeAsset<TOutput> output = new BiomeAsset<TOutput>()
            {
                DefaultBiome = DefaultBiome,
                DefaultOutput = DefaultOutput,
            };

            foreach (KeyValuePair<int, PropertyMatcher<TOutput>> pair in _biomeBlocks)
            {
                output._biomeBlocks.Add(pair.Key, pair.Value);
            }

            foreach (int blockHash in _blockCache)
            {
                output._blockCache.Add(blockHash);
            }

            return output;
        }
    }
}
