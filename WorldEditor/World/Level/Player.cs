using System.Numerics;

namespace WorldEditor
{
    public struct Player
    {
        public Vector3 Position { get; set; }
        public Vector3 Spawn { get; set; }
        public Dimension Dimension { get; set; }
        public GameType GameType { get; set; }
    }
}
