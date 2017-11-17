using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;
using ZombieGame.Physics.Enums;
using ZombieGame.Physics.Events;
using ZombieGame.Physics.Extensions;

namespace ZombieGame.Game.Prefabs.Entities
{
    public sealed class Wall : Entity
    {
        public WallTypes Type { get; private set; }

        public Wall(WallTypes type) : base("Wall", Tags.Wall)
        {
            Type = type;
            Sprite.Uri = Sprite.TransparentSprite;
            Show();
        }

        protected override void Update()
        {
            if (Type == WallTypes.BottomWall)
                RigidBody.SetPosition(new Vector(GameMaster.Camera.RigidBody.Position.X, GameMaster.Camera.RigidBody.Position.Y + -GameMaster.Camera.RigidBody.Size.Y));
            else if (Type == WallTypes.TopWall)
                RigidBody.SetPosition(new Vector(GameMaster.Camera.RigidBody.Position.X, GameMaster.Camera.RigidBody.Position.Y + RigidBody.Size.Y));
            else if (Type == WallTypes.LeftWall)
                RigidBody.SetPosition(new Vector(GameMaster.Camera.RigidBody.Position.X - RigidBody.Size.X, GameMaster.Camera.RigidBody.Position.Y));
            else if (Type == WallTypes.RightWall)
                RigidBody.SetPosition(new Vector(GameMaster.Camera.RigidBody.Position.X + GameMaster.Camera.RigidBody.Size.X, GameMaster.Camera.RigidBody.Position.Y));

            base.Update();
        }

        protected override void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            // Garantir que nada seja feito
        }

        protected override void OnCollisionStay(object sender, CollisionEventArgs e)
        {
            if (e.Collider.Tag != Tags.Player)
                return;

            if (Type == WallTypes.BottomWall && e.Collider.Tag != Tags.Wall)
            {
                if (e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterBottom).Y < RigidBody.Bounds.GetVector(RectPositions.CenterTop).Y &&
                    e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterBottom).Y > RigidBody.Bounds.GetVector(RectPositions.CenterBottom).Y)
                    e.Collider.RigidBody.SetPosition(new Vector(e.Collider.RigidBody.Position.X, RigidBody.Position.Y + e.Collider.RigidBody.Size.Y));
            }
            else if (Type == WallTypes.TopWall && e.Collider.Tag != Tags.Wall)
            {
                if (e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterTop).Y > RigidBody.Bounds.GetVector(RectPositions.CenterBottom).Y &&
                    e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterTop).Y < RigidBody.Bounds.GetVector(RectPositions.CenterTop).Y)
                    e.Collider.RigidBody.SetPosition(new Vector(e.Collider.RigidBody.Position.X, RigidBody.Position.Y - RigidBody.Size.Y));
            }
            else if (Type == WallTypes.RightWall && e.Collider.Tag != Tags.Wall)
            {
                if (e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterRight).X > RigidBody.Bounds.GetVector(RectPositions.CenterLeft).X &&
                    e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterRight).X < RigidBody.Bounds.GetVector(RectPositions.CenterRight).X)
                    e.Collider.RigidBody.SetPosition(new Vector(RigidBody.Position.X - e.Collider.RigidBody.Size.X, e.Collider.RigidBody.Position.Y));
            }
            else if (Type == WallTypes.LeftWall && e.Collider.Tag != Tags.Wall)
            {
                if (e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterLeft).X < RigidBody.Bounds.GetVector(RectPositions.CenterRight).X &&
                    e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterLeft).X > RigidBody.Bounds.GetVector(RectPositions.CenterLeft).X)
                    e.Collider.RigidBody.SetPosition(new Vector(RigidBody.Position.X + RigidBody.Size.X, e.Collider.RigidBody.Position.Y));
            }
            if (e.Collider.RigidBody.Force.Magnitude > 0)
                e.Collider.RigidBody.SetForce(Vector.Zero);
        }

        protected override void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            // Garantir que nada seja feito
        }

    }
}
