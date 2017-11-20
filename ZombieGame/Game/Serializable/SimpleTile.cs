using System;
using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Interfaces;
using ZombieGame.Physics;

namespace ZombieGame.Game.Serializable
{
    public class SimpleTile : ISerializableTile
    {
        public string Name { get; set; }
        public float Mass { get; set; }
        public Vector Size { get; set; }
        public TileType Type { get; set; }
        public string SpriteFileName { get; set; }
        public Vector Position { get; set; }
        public float Rotation { get; set; }

        public SimpleTile() { }

        public Tile Mount()
        {
            return Tile.Mount(this);
        }
    }
}
