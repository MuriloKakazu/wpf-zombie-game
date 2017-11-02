using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public class Entity
    {
        public string Name { get; set; }
        public Tags Tag { get; set; }
        public RigidBody RigidBody { get; set; }
        public Collider Collider { get; set; }

        public void OnCollision(Entity collider)
        {
            if (Tag == Tags.Enemy)
            {

            }
            else if (Tag == Tags.Player)
            {

            }
        }
    }
}
