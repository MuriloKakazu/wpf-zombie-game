using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;
using ZombieGame.Physics.Enums;
using ZombieGame.Physics.Events;
using ZombieGame.Physics.Extensions;

namespace ZombieGame.Game.Prefabs.OtherEntities
{
    public sealed class Wall : Entity
    {
        public WallTypes Type { get; private set; }

        public Wall(WallTypes type) : base("Wall", Tags.Wall)
        {
            Type = type;
        }

        protected override void CheckCollision()
        {
            base.CheckCollision();
        }

        protected override void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            base.OnCollisionEnter(sender, e);
        }

        protected override void OnCollisionStay(object sender, CollisionEventArgs e)
        {
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
        }

        protected override void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            base.OnCollisionLeave(sender, e);
        }

    }
}
