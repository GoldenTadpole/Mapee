using System.Collections.Concurrent;
using WorldEditor;

namespace Mapper
{
    public class StepProvider : IStepProvider
    {
        private IDictionary<Coords, StepChunk> _stepSectors;

        public StepProvider()
        {
            _stepSectors = new ConcurrentDictionary<Coords, StepChunk>();
        }

        public void Add(int x, int z, StepChunk chunk)
        {
            _stepSectors.TryAdd(new Coords(x, z), chunk);
        }
        public void Remove(int x, int z)
        {
            _stepSectors.Remove(new Coords(x, z));
        }

        public void Clear()
        {
            _stepSectors.Clear();
        }

        public short[]? ProvideStepStrip(int x, int z, Direction direction)
        {
            short[]? chunk = ProvideStepChunk(x, z);
            if (chunk is null) return null;

            short[] output = new short[16];
            switch (direction)
            {
                case Direction.East:
                    for (int i = 0; i < 16; i++)
                    {
                        output[i] = chunk[i * 16 + 15];
                    }
                    break;
                case Direction.West:
                    for (int i = 0; i < 16; i++)
                    {
                        output[i] = chunk[i * 16];
                    }
                    break;
                case Direction.South:
                    for (int i = 0; i < 16; i++)
                    {
                        output[i] = chunk[16 * 15 + i];
                    }
                    break;
                case Direction.North:
                    for (int i = 0; i < 16; i++)
                    {
                        output[i] = chunk[i];
                    }
                    break;
            }

            return output;
        }
        public short[]? ProvideStepChunk(int x, int z)
        {
            if (!_stepSectors.TryGetValue(new Coords(x, z), out StepChunk? chunk)) return null;
            return chunk.Steps;
        }
    }
}
