using MapScanner;
using WorldEditor;

namespace Mapper
{
    public class BlockController : IBlockController
    {
        private IDictionary<int, RenderBlockSection> _renderBlockSections;

        private RenderBlockSection? _lastSection;
        private int _lastSectionY = int.MinValue;

        private short[]? _stepChunk;
        private short[]? _xPosStrip;
        private short[]? _xNegStrip;
        private short[]? _zPosStrip;
        private short[]? _zNegStrip;

        public BlockController(IScannedChunk scannedChunk, AssetPack assetPack, IStepProvider stepProvider)
        {
            if (scannedChunk.BlockSections is null) throw new ArgumentNullException();

            _renderBlockSections = new Dictionary<int, RenderBlockSection>(scannedChunk.BlockSections.Length);
            for (int i = 0; i < scannedChunk.BlockSections.Length; i++)
            {
                if (scannedChunk.BlockSections[i].Palette == null) continue;

                _renderBlockSections.Add(scannedChunk.BlockSections[i].Y, new RenderBlockSection(assetPack, scannedChunk.BlockSections[i], scannedChunk.BiomeSections?[i]));
            }

            _stepChunk = stepProvider.ProvideStepChunk(scannedChunk.Coords.X, scannedChunk.Coords.Z);
            _xPosStrip = stepProvider.ProvideStepStrip(scannedChunk.Coords.X + 1, scannedChunk.Coords.Z, Direction.West);
            _xNegStrip = stepProvider.ProvideStepStrip(scannedChunk.Coords.X - 1, scannedChunk.Coords.Z, Direction.East);
            _zPosStrip = stepProvider.ProvideStepStrip(scannedChunk.Coords.X, scannedChunk.Coords.Z + 1, Direction.North);
            _zNegStrip = stepProvider.ProvideStepStrip(scannedChunk.Coords.X, scannedChunk.Coords.Z - 1, Direction.South);
        }

        public RenderBlock GetRenderBlock(ScannedBlock block)
        {
            RenderBlockSection? section = GetSection(block.FirstInstanceY);
            if (section is null) return new RenderBlock();

            return section.ProvideRenderBlock(block.Data);
        }
        public DepthOpacity GetDepthOpacity(ScannedBlock block)
        {
            RenderBlockSection? section = GetSection(block.FirstInstanceY);
            if (section is null) return new DepthOpacity();

            return section.ProvideDepthOpacity(block.Data);
        }

        public Step GetStep(Coords coords)
        {
            if (_stepChunk == null) return new Step();

            short x = (short)coords.X, y = _stepChunk[coords.X + coords.Z * 16], z = (short)coords.Z;
            short xPos = y, xNeg = y, zPos = y, zNeg = y;

            if (x == 0)
            {
                xPos = GetXPos();
                if (_xNegStrip != null) xNeg = _xNegStrip[z];
            }
            else if (x == 15)
            {
                if (_xPosStrip != null) xPos = _xPosStrip[z];
                xNeg = GetXNeg();
            }
            else
            {
                xPos = GetXPos();
                xNeg = GetXNeg();
            }

            if (z == 0)
            {
                zPos = GetZPos();
                if (_zNegStrip != null) zNeg = _zNegStrip[x];
            }
            else if (z == 15)
            {
                if (_zPosStrip != null) zPos = _zPosStrip[x];
                zNeg = GetZNeg();
            }
            else
            {
                zPos = GetZPos();
                zNeg = GetZNeg();
            }

            short GetXPos() => _stepChunk[x + 1 + z * 16];
            short GetXNeg() => _stepChunk[x - 1 + z * 16];
            short GetZPos() => _stepChunk[x + (z + 1) * 16];
            short GetZNeg() => _stepChunk[x + (z - 1) * 16];

            return new Step(y, xPos, xNeg, zPos, zNeg);
        }
        public StepSettings GetStepSettings(ScannedBlock block)
        {
            RenderBlockSection? section = GetSection(block.FirstInstanceY);
            if (section is null) return new StepSettings();

            return section.ProvideStepSettings(block.Data);
        }

        private RenderBlockSection? GetSection(int y)
        {
            int sectionY = MathUtilities.FindSectionY(y);
            if (_lastSection is null || _lastSectionY != sectionY)
            {
                _renderBlockSections.TryGetValue(sectionY, out _lastSection);
            }

            return _lastSection;
        }

        public void Dispose()
        {
            foreach (var section in _renderBlockSections)
            {
                section.Value.Dispose();
            }
            _renderBlockSections.Clear();
        }
    }
}
