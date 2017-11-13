using ZombieGame.Game.Enums;
using ZombieGame.Game.Interfaces;

namespace ZombieGame.Game.Entities
{
    public class Tile : Entity
    {
        public TileTypes Type { get; protected set; }

        public Tile(TileTypes type) : base("Tile", Tags.Tile)
        {
            if (type == TileTypes.BackTile)
                SetZIndex(1);
            else if (type == TileTypes.ForeTile)
                SetZIndex(5);
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
