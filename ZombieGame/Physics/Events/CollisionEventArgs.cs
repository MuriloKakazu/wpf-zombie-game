using System;
using ZombieGame.Game;

namespace ZombieGame.Physics.Events
{
    public class CollisionEventArgs : EventArgs
    {
        /// <summary>
        /// Entidade com a qual se colidiu
        /// </summary>
        public Entity Collider { get; protected set; }
        /// <summary>
        /// Direção da colisão
        /// </summary>
        public Vector CollisionDirection { get; protected set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="collider">Entidade com a qual se colidiu</param>
        /// <param name="collisionDirection">Direção da colisão</param>
        public CollisionEventArgs(Entity collider, Vector collisionDirection)
        {
            Collider = collider;
            CollisionDirection = collisionDirection;
        }
    }
}
