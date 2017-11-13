using System;
using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;
using ZombieGame.Physics.Events;

namespace ZombieGame.Game.Prefabs.Entities
{
    public sealed class Camera : Entity
    {
        public Camera() : base("Camera", Tags.Camera)
        {
            RigidBody.SetPosition(Vector.Zero);
            RigidBody.Resize(Vector.WindowSize);
            RigidBody.Freeze();
            RigidBody.UseRotation = true;
            RigidBody.SetRotation(45);
            RigidBody.IgnoreCollisions = false;
            Sprite.Uri = Sprite.TransparentSprite;
            Show();
            SetZIndex(7);
        }

        protected override void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            if (e.Collider.Tag != Tags.Wall && !e.Collider.IsPlayer)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Destroying object: {0}", e.Collider.Name);
                Console.ResetColor();
                e.Collider.Destroy();
            }
            else if (e.Collider.IsPlayer)
            {
                Console.WriteLine("Respawning {0}", e.Collider.Name);
                e.Collider.RigidBody.SetPosition(new Vector(RigidBody.CenterPoint.X, RigidBody.CenterPoint.Y));
                e.Collider.RigidBody.SetVelocity(Vector.Zero);
                e.Collider.RigidBody.SetForce(Vector.Zero);
            }
        }

        protected override void Update()
        {
            if (GameMaster.GetPlayer(0) != null)
            {
                //RigidBody.SetPosition(new Vector(
                //    (GameMaster.GetPlayer(0).Character.RigidBody.CenterPoint.X + GameMaster.GetPlayer(1).Character.RigidBody.CenterPoint.X) / 2 - Vector.WindowSize.X / 2,
                //    (GameMaster.GetPlayer(0).Character.RigidBody.CenterPoint.Y + GameMaster.GetPlayer(1).Character.RigidBody.CenterPoint.Y) / 2 + Vector.WindowSize.Y / 2));
                var newPos = new Vector();
                var p1Pos = GameMaster.GetPlayer(0).Character.RigidBody.CenterPoint;
                var p2Pos = GameMaster.GetPlayer(1).Character.RigidBody.CenterPoint;
                var newX = (p1Pos.X + p2Pos.X) / 2 - Vector.WindowSize.X / 2;
                var newY = (p1Pos.Y + p2Pos.Y) / 2 + Vector.WindowSize.Y / 2;

                if (newX > -1274 && newX < 1274)
                    newPos.X = newX;
                else
                    newPos.X = RigidBody.Position.X;

                if (-newY > -691 && -newY < 1382)
                    newPos.Y = newY;
                else
                    newPos.Y = RigidBody.Position.Y;

                RigidBody.SetPosition(newPos);
            }
            base.Update();
        }

        protected override void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            // Garantir que nada seja feito
        }

        protected override void OnCollisionStay(object sender, CollisionEventArgs e)
        {
            // Garantir que nada seja feito
        }
    }
}
