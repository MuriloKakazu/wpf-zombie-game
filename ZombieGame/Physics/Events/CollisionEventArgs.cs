using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game;

namespace ZombieGame.Physics.Events
{
    public class CollisionEventArgs
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
