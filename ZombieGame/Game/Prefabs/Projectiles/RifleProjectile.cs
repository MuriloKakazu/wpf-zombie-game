using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;

namespace ZombieGame.Game.Prefabs.Projectiles
{
    public sealed class RifleProjectile : Projectile
    {
        public RifleProjectile() : base(ProjectileTypes.RifleProjectile)
        {
            this.HitDamage = 5;
        }
    }
}
