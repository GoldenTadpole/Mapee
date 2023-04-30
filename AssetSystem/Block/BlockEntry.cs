namespace AssetSystem.Block
{
    public class BlockEntry<TOutput> : ICloneable where TOutput : struct
    {
        public IList<PropertyMatcher<TOutput>> Evaluators { get; private set; }
        public TOutput? DefaultValue { get; set; }
        public string BlockName { get; private set; }

        public BlockEntry(string blockName, int capacity = 0)
        {
            Evaluators = new List<PropertyMatcher<TOutput>>(capacity);
            BlockName = blockName;
        }

        public bool Provide(WorldEditor.Property[] properties, out TOutput output)
        {
            if (Evaluators.Count == 0 && (properties is null || properties.Length == 0)) 
            {
                output = DefaultValue ?? default;
                return true;
            }

            PropertyValueProvider propertyValueGetter = PropertyValueProviderUtilities.CreateGetter(properties);
            for (int i = 0; i < Evaluators.Count; i++)
            {
                PropertyMatcher<TOutput> evaluator = Evaluators[i];

                if (evaluator.Match(propertyValueGetter))
                {
                    output = evaluator.Payload;
                    return true;
                }
            }

            output = DefaultValue ?? default;
            return DefaultValue is not null;
        }

        public object Clone()
        {
            BlockEntry<TOutput> output = new BlockEntry<TOutput>(BlockName)
            {
                DefaultValue = DefaultValue
            };

            foreach (PropertyMatcher<TOutput> evaluator in Evaluators) 
            {
                output.Evaluators.Add((PropertyMatcher<TOutput>) evaluator.Clone());
            }

            return output;
        }
    }
}
