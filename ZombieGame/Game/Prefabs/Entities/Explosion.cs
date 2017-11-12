using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game.Prefabs.Entities
{
    public class Explosion : AnimatedEntity
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pos">Posição</param>
        /// <param name="size">Tamanho</param>
        public Explosion(Vector pos, Vector size, float duration) : base("Explosion", Tags.VisualFX)
        {
            Spritesheet.LoadFrom(IO.GlobalPaths.AnimatedSprites + "explosion/");
            Duration = duration;
            DestroyOnAnimationEnd = true;
            RigidBody.IgnoreCollisions = true;
            RigidBody.SetPosition(pos);
            RigidBody.Resize(size);
            SetTimer();
        }
    }
}
