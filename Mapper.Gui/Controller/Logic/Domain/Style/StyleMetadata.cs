using AssetSystem;
using System.Windows.Media;

namespace Mapper.Gui.Logic
{
    public class StyleMetadata
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public int OrderedIndex { get; set; }
        public bool AutomaticallyDisableNight { get; set; }
        public bool DisableDatapackStyle { get; set; }
        public LogicalExpression DimensionChecker { get; set; }
        public ImageSource? Icon { get; set; }

        public StyleMetadata(string name, string id, int orderedIndex, LogicalExpression dimensionChecker) 
        {
            Name = name;
            Id = id;
            OrderedIndex = orderedIndex;
            DimensionChecker = dimensionChecker;
        }
    }
}
