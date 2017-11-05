using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;

namespace ZombieGame.Game.Prefabs.Projectiles
{
    public sealed class HeavyMGProjectile : Projectile
    {
        public HeavyMGProjectile() : base(ProjectileTypes.HeavyMGProjectile)
        {
            this.HitDamage = 7;
        }
    }
}
