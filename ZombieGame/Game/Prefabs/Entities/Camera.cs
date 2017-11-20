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
        public static Camera Instance { get; set; }
        public static Wall BottomWall { get; set; }
        public static Wall TopWall { get; set; }
        public static Wall LeftWall { get; set; }
        public static Wall RightWall { get; set; }

        public Camera() : base("Camera", Tag.Camera)
        {
            Instance = this;
            RigidBody.SetPosition(Vector.Zero);
            RigidBody.Resize(Vector.WindowSize);
            RigidBody.Freeze();
            RigidBody.UseRotation = true;
            RigidBody.SetRotation(45);
            RigidBody.IgnoreCollisions = false;
            Sprite = new TransparentSprite();
            SetupWalls();
            Show();
            SetZIndex(Enums.ZIndex.Camera);
        }

        private void SetupWalls()
        {
            BottomWall = new Wall(WallType.BottomWall);
            BottomWall.RigidBody.SetPosition(new Vector(RigidBody.Position.X, RigidBody.Position.Y - RigidBody.Size.Y));
            BottomWall.RigidBody.Resize(new Vector(RigidBody.Size.X, 100));
            BottomWall.RigidBody.Freeze();
            TopWall = new Wall(WallType.TopWall);
            TopWall.RigidBody.SetPosition(new Vector(RigidBody.Position.X, RigidBody.Position.Y));
            TopWall.RigidBody.Resize(new Vector(RigidBody.Size.X, 100));
            TopWall.RigidBody.Freeze();
            LeftWall = new Wall(WallType.LeftWall);
            LeftWall.RigidBody.SetPosition(new Vector(RigidBody.Position.X, RigidBody.Position.Y));
            LeftWall.RigidBody.Resize(new Vector(100, RigidBody.Size.Y));
            LeftWall.RigidBody.Freeze();
            RightWall = new Wall(WallType.RightWall);
            RightWall.RigidBody.SetPosition(new Vector(RigidBody.Size.X, 0));
            RightWall.RigidBody.Resize(new Vector(100, RigidBody.Size.Y));
            RightWall.RigidBody.Freeze();
        }

        public static Vector GetTopLeftFocusPoint()
        {
            var player1 = GameMaster.GetPlayer(0);
            var player2 = GameMaster.GetPlayer(1);
            var scene = GameMaster.CurrentScene;
            var newPos = new Vector();
            var p1Pos = player1.Character.RigidBody.CenterPoint;
            var p2Pos = player2.Character.RigidBody.CenterPoint;
            float newX = 0;
            float newY = 0;

            if (player1.Character != null && player2.Character != null && player1.Character.IsActive && player2.Character.IsActive)
            {
                newX = (p1Pos.X + p2Pos.X) / 2 - Vector.WindowSize.X / 2;
                newY = (p1Pos.Y + p2Pos.Y) / 2 + Vector.WindowSize.Y / 2;
            }
            else
            {
                Vector alivePlayerPos = null;
                if (player1.Character == null || !player1.Character.IsActive)
                    alivePlayerPos = player2.Character.RigidBody.CenterPoint;
                else if (player2.Character == null || !player2.Character.IsActive)
                    alivePlayerPos = player1.Character.RigidBody.CenterPoint;

                newX = alivePlayerPos.X - Instance.RigidBody.Size.X / 2;
                newY = alivePlayerPos.Y + Instance.RigidBody.Size.Y / 2;
            }

            if (newX > scene.RenderPosition.X && newX < scene.Size.X + scene.RenderPosition.X * 2)
                newPos.X = newX;
            else
                newPos.X = Instance.RigidBody.Position.X;

            if (-newY > -scene.RenderPosition.Y && -newY < scene.Size.Y - scene.RenderPosition.Y * 2)
                newPos.Y = newY;
            else
                newPos.Y = Instance.RigidBody.Position.Y;

            return newPos;
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
            RigidBody.SetPosition(GetTopLeftFocusPoint());
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
