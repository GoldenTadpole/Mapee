namespace Mapper
{
    public readonly struct Step
    {
        public short BaseY { get; init; }

        public short XPos { get; init; }
        public short XNeg { get; init; }
        public short ZPos { get; init; }
        public short ZNeg { get; init; }

        public Step(short y, short xPos, short xNeg, short zPos, short zNeg)
        {
            BaseY = y;

            XPos = FindPos(xPos);
            XNeg = FindPos(xNeg);
            ZPos = FindPos(zPos);
            ZNeg = FindPos(zNeg);

            short FindPos(float pos) => pos != short.MinValue ? (short)(y - pos) : y;
        }
    }
}
