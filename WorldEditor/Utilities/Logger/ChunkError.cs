namespace WorldEditor
{
    public class ChunkError
    {
        public Exception Exception { get; set; }
        public Coords Coords { get; set; }

        public ChunkError(Exception exception, Coords coords)
        {
            Exception = exception;
            Coords = coords;
        }

        public override string ToString()
        {
            return $"Error at chunk [X = {Coords.X}, Z = {Coords.Z}], {Exception}";
        }
    }
}
