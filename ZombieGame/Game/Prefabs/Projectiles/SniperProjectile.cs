using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;

namespace ZombieGame.Game.Prefabs.Projectiles
{
    public sealed class SniperProjectile : Projectile
    {
        public SniperProjectile() : base(ProjectileTypes.SniperProjectile)
        {
            this.HitDamage = 50;
        }
    }
}
