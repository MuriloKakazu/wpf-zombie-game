using System;
using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Prefabs.Sprites;
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
            Sprite = new TransparentSprite();
            Show();
            SetZIndex(ZIndexes.Camera);
        }

        protected override void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            if (e.Collider.IsPlayer)
            {
                Console.WriteLine("Respawning {0}", e.Collider.Name);
                e.Collider.RigidBody.SetPosition(new Vector(RigidBody.CenterPoint.X, RigidBody.CenterPoint.Y));
                e.Collider.RigidBody.SetVelocity(Vector.Zero);
                e.Collider.RigidBody.SetForce(Vector.Zero);
            }
        }

        protected override void FixedUpdate()
        {
            var player1 = GameMaster.GetPlayer(0);
            var player2 = GameMaster.GetPlayer(1);
            if (player1 != null)
            {

                var scene = GameMaster.CurrentScene;
                var newPos = new Vector();
                var p1Pos = player1.Character.RigidBody.CenterPoint;

                if (player2.IsPlaying && player2.Character != null && player2.Character.IsActive)
                {
                    var p2Pos = player2.Character.RigidBody.CenterPoint;
                    var newX = (p1Pos.X + p2Pos.X) / 2 - Vector.WindowSize.X / 2;
                    var newY = (p1Pos.Y + p2Pos.Y) / 2 + Vector.WindowSize.Y / 2;

                    if (newX > scene.RenderPosition.X && newX < scene.Size.X + scene.RenderPosition.X * 2)
                        newPos.X = newX;
                    else
                        newPos.X = RigidBody.Position.X;

                    if (-newY > -scene.RenderPosition.Y && -newY < scene.Size.Y - scene.RenderPosition.Y * 2)
                        newPos.Y = newY;
                    else
                        newPos.Y = RigidBody.Position.Y;
                }
                else
                {
                    var newX = p1Pos.X - RigidBody.Size.X / 2;
                    var newY = p1Pos.Y + RigidBody.Size.Y / 2;

                    if (newX > scene.RenderPosition.X && newX < scene.Size.X + scene.RenderPosition.X * 2)
                        newPos.X = newX;
                    else
                        newPos.X = RigidBody.Position.X;

                    if (-newY > -scene.RenderPosition.Y && -newY < scene.Size.Y - scene.RenderPosition.Y * 2)
                        newPos.Y = newY;
                    else
                        newPos.Y = RigidBody.Position.Y;
                }

                RigidBody.SetPosition(newPos);
            }
            base.FixedUpdate();
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
