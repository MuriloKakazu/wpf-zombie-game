using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.IO;

namespace ZombieGame.Game.Prefabs.Characters
{
    class Zombie : Enemy
    {
        public Zombie() : base(EnemyTypes.Zombie)
        {
            LoadSprite(GlobalPaths.CharacterSprites + "zombie.png");
            RigidBody.Resize(new Physics.Vector(50, 50));
        }
    }
}
