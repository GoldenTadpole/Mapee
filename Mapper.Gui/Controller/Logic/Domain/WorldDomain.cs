using System.Collections.Generic;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class WorldDomain
    {
        public Style Style { get; set; }
        public DataPackAssetPack? DataPack { get; set; }
        public Level Level { get; set; }

        public DimensionDomain CurrentDimension
        {
            get => _dimensions[_currentDimension];
            set
            {
                SetDimension(value);
                _currentDimension = value.Dimension;
            }
        }
        private Dimension _currentDimension;

        public IEnumerable<DimensionDomain> Dimensions => _dimensions.Values;

        private readonly IDictionary<Dimension, DimensionDomain> _dimensions = new Dictionary<Dimension, DimensionDomain>(3);

        public WorldDomain(Style style, Level level, DimensionDomain dimension)
        {
            Style = style;
            Level = level;
            CurrentDimension = dimension;
        }

        public DimensionDomain? GetDimension(Dimension dimension)
        {
            _ = _dimensions.TryGetValue(dimension, out DimensionDomain? domain);
            return domain;
        }
        public void SetDimension(DimensionDomain dimensionDomain)
        {
            if (!_dimensions.ContainsKey(dimensionDomain.Dimension))
            {
                _dimensions.Add(dimensionDomain.Dimension, dimensionDomain);
            }
            else
            {
                _dimensions[dimensionDomain.Dimension] = dimensionDomain;
            }
        }
    }
}
