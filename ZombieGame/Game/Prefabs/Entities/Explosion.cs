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
        /// ctor
        /// </summary>
        /// <param name="pos">Posição</param>
        /// <param name="size">Tamanho</param>
        public Explosion(Vector pos, Vector size, float duration) : base("Explosion", Tags.VisualFX)
        {
            Spritesheet.LoadFrom(IO.GlobalPaths.AnimatedSprites + "explosion/");
            AnimationDuration = duration;
            DestroyOnAnimationEnd = true;
            RigidBody.IgnoreCollisions = true;
            RigidBody.SetPosition(pos);
            RigidBody.Resize(size);
            RigidBody.SetRotation(Random.Next(0, 360));
            UpdateVisualControl();
            SetZIndex(ZIndexes.VisualFX);
            SetTimer();
        }
    }
}
