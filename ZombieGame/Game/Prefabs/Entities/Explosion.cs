using System;
using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game.Prefabs.Entities
{
    public class Explosion : AnimatedEntity
    {
        private static Random Random = new Random();

        /// <summary>
        /// Cria uma explosão no cenário
        /// </summary>
        /// <param name="pos">Posição da explosão</param>
        /// <param name="radius">Raio da explosão</param>
        public static void Create(Vector pos, float radius)
        {
            new Explosion(new Vector(pos.X - radius / 2, pos.Y + radius / 2), new Vector(radius, radius), 750).Show();
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pos">Posição</param>
        /// <param name="size">Tamanho</param>
        public Explosion(Vector pos, Vector size, float duration) : base("Explosion", Tag.VisualFX)
        {
            Spritesheet.LoadFrom(IO.GlobalPaths.AnimatedSprites + "explosion/");
            AnimationDuration = duration;
            DestroyOnAnimationEnd = true;
            RigidBody.IgnoreCollisions = true;
            RigidBody.SetPosition(pos);
            RigidBody.Resize(size);
            RigidBody.SetRotation(Random.Next(0, 360));
            UpdateVisualControl();
            base.SetZIndex(Enums.ZIndex.VisualFX);
            SetTimer();
        }
    }
}
