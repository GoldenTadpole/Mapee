namespace Mapper
{
    public class StepChunk
    {
        public short[] Steps { get; set; }

        public StepChunk()
        {
            Steps = new short[256];
        }
    }
}
