using Mapper.Gui.Model;
using System.Windows.Media;

namespace Mapper.Gui.Logic
{
    public class StyleAdapter : IStyle
    {
        public string Name => BaseImplementation.Metadata.Name;
        public string Id => BaseImplementation.Metadata.Id;
        public ImageSource Icon => BaseImplementation.Metadata.Icon;

        public Style BaseImplementation { get; }

        public StyleAdapter(Style baseImplementation) 
        {
            BaseImplementation = baseImplementation;
        }
    }
}
