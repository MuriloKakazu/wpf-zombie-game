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

        public Camera() : base("Camera", Tag.Camera)
        {
            Instance = this;
            RigidBody.SetPosition(Vector.Zero);
            RigidBody.Resize(Vector.WindowSize);
            RigidBody.Freeze();
            RigidBody.UseRotation = true;
            RigidBody.SetRotation(45);
            Sprite = new TransparentSprite();
            SetupWalls();
            Show();
        }

        private void SetupWalls()
        {
            var sizeX = RigidBody.Size.X;
            var sizeY = RigidBody.Size.Y;
            var fixedSize = 100;

            var bottomWall = new Wall(WallType.BottomWall);
            bottomWall.RigidBody.Resize(new Vector(sizeX, fixedSize));
            var topWall = new Wall(WallType.TopWall);
            topWall.RigidBody.Resize(new Vector(sizeX, fixedSize));
            var leftWall = new Wall(WallType.LeftWall);
            leftWall.RigidBody.Resize(new Vector(fixedSize, sizeY));
            var rightWall = new Wall(WallType.RightWall);
            rightWall.RigidBody.Resize(new Vector(fixedSize, sizeY));
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
                e.Collider.RigidBody.SetPosition(new Vector(RigidBody.CenterPoint.X, RigidBody.CenterPoint.Y));
                e.Collider.RigidBody.Stop();
            }
        }

        public void ForceUpdate()
        {
            RigidBody.SetPosition(GetTopLeftFocusPoint());
            base.FixedUpdate();
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
