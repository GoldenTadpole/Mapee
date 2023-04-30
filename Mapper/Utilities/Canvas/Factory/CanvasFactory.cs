using CommonUtilities.Factory;

namespace Mapper
{
    public class CanvasFactory : IFactory<CanvasArgs, ICanvas>
    {
        public ICanvas Create(CanvasArgs parameter)
        {
            return new Canvas(parameter.TopLeft, parameter.Size, parameter.Direction);
        }
    }
}
