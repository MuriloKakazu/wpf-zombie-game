using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public class Weapon
    {
        public float Damage { get; set; }
        public float FireRate { get; set; } //bullets per minute
        public WeaponTypes Type { get; set; }
    }
}
