using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;

namespace ZombieGame.Game.Prefabs.Characters
{
    class Boss : Enemy
    {
        public Boss() : base(EnemyTypes.Boss)
        {
            
        }
    }
}
