using MapScanner;
using WorldEditor;

namespace Mapper
{
    public interface IBlockController : IDisposable
    {
        RenderBlock GetRenderBlock(ScannedBlock block);
        DepthOpacity GetDepthOpacity(ScannedBlock block);

        Step GetStep(Coords coords);
        StepSettings GetStepSettings(ScannedBlock block);
    }
}
