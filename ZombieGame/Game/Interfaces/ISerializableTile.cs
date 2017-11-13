using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game.Interfaces
{
    public interface ISerializableTile : ISerializableEntity
    {
        TileTypes Type { get; set; }
        Vector Position { get; set; }
        float Rotation { get; set; }

        Tile Mount();
    }
}
