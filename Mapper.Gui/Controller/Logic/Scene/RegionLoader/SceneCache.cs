using Mapper.Gui.Model;

namespace Mapper.Gui.Logic
{
    public class SceneCache
    {
        public XzPoint TopLeft { get; set; }
        public XzPoint BottomRight { get; set; }
        public XzRange PrevRange { get; set; }

        public SceneCache() 
        {
            Reset();
        }

        public void Reset() 
        {
            TopLeft = new(double.MaxValue, double.MaxValue);
            BottomRight = new(double.MinValue, double.MinValue);
            PrevRange = XzRange.Empty;
        }
    }
}
