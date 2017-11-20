using ZombieGame.Game.Enums;
using ZombieGame.Game.Interfaces;

namespace ZombieGame.Game.Entities
{
    public class Tile : Entity
    {
        public TileType Type { get; protected set; }

        public Tile(TileType type) : base("Tile", Tag.Tile)
        {
            if (type == TileType.BackTile)
                base.SetZIndex(Enums.ZIndex.BackTile);
            else if (type == TileType.ForeTile)
                base.SetZIndex(Enums.ZIndex.ForeTile);
            RigidBody.UseRotation = false;
            RigidBody.Freeze();
            RigidBody.IgnoreCollisions = true;
        }

        public static Tile Mount(ISerializableTile source)
        {
            Tile t = new Tile(source.Type) { Name = source.Name };
            t.Sprite.Uri = IO.GlobalPaths.TileSprites + source.SpriteFileName;
            t.RigidBody.SetMass(source.Mass);
            t.RigidBody.Resize(source.Size);
            t.RigidBody.SetPosition(source.Position);
            t.RigidBody.SetRotation(source.Rotation);
            return t;
        }
    }
}
