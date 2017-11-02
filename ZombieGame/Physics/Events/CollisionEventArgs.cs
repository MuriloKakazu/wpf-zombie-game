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
        public Entity Collider { get; protected set; }
        public Vector CollisionDirection { get; protected set; }

        public CollisionEventArgs(Entity collider, Vector collisionDirection)
        {
            Collider = collider;
            CollisionDirection = collisionDirection;
        }
    }
}
