using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.IO;

namespace ZombieGame.Game.Prefabs.Weapons
{
    public sealed class RPG : Weapon
    {
        public RPG() : base(WeaponTypes.Explosive, ProjectileTypes.Missile)
        {
            FireRate = 1000f;
        }
    }
}
