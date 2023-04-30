using NbtEditor;
using System.Numerics;

namespace WorldEditor
{
    public class PlayerReader : IObjectReader<LevelArgs, Player>
    {
        public Player Read(LevelArgs input)
        {
            Player player = new Player();
            if (!input.Data.TryGetChild("Player", out Tag playerTag) || playerTag is not CompoundTag playerCompound) return player;

            ReadPlayerPositions(playerCompound, out Vector3 pos, out Vector3 spawn);

            return new Player()
            {
                Position = pos,
                Spawn = spawn,
                Dimension = ReadPlayerDimension(playerCompound),
                GameType = ReadPlayerGameType(playerCompound)
            };
        }

        private static void ReadPlayerPositions(CompoundTag player, out Vector3 pos, out Vector3 spawn)
        {
            if (player.TryGetChild("Pos", out Tag posTag) && posTag is ListTag posArray)
            {
                pos = new Vector3((float)(double)posArray[0], (float)(double)posArray[1], (float)(double)posArray[2]);
            }
            else
            {
                pos = new Vector3();
            }

            int x = 0, y = 0, z = 0;
            if (player.TryGetChild("SpawnX", out Tag xTag) && xTag is ValueTag<int> xValueTag) 
            {
                x = xValueTag;
            }
            if (player.TryGetChild("SpawnY", out Tag yTag) && yTag is ValueTag<int> yValueTag)
            {
                y = yValueTag;
            }
            if (player.TryGetChild("SpawnZ", out Tag zTag) && zTag is ValueTag<int> zValueTag)
            {
                z = zValueTag;
            }

            spawn = new Vector3(x, y, z);
        }
        private static Dimension ReadPlayerDimension(CompoundTag player) 
        {
            Tag? dimensionTag = player["Dimension"];
            if (dimensionTag is null) return Dimension.Overworld;

            if (dimensionTag.Id == TagId.Int32) 
            {
                return (int)dimensionTag switch
                {
                    0 => Dimension.Overworld,
                    1 => Dimension.Nether,
                    2 => Dimension.TheEnd,
                    _ => Dimension.Overworld
                };
            }

            if (dimensionTag.Id == TagId.String) 
            {
                string dimension = dimensionTag;
                return dimension switch
                {
                    "minecraft:overworld" => Dimension.Overworld,
                    "minecraft:nether" => Dimension.Nether,
                    "minecraft:end" => Dimension.TheEnd,
                    _ => Dimension.Overworld
                };
            }

            return Dimension.Overworld;
        }
        private static GameType ReadPlayerGameType(CompoundTag player) 
        {
            Tag? gameTypeTag = player["playerGameType"];
            if (gameTypeTag is null || gameTypeTag.Id != TagId.Int32) return GameType.Survival;

            return (GameType)(int)gameTypeTag;
        }
    }
}
