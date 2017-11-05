using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.IO;

namespace ZombieGame.Game.Prefabs.Projectiles
{
    public sealed class PistolProjectile : Projectile
    {
        public PistolProjectile() : base(ProjectileTypes.PistolProjectile)
        {
            this.HitDamage = 1;
            this.SpeedMagnitude = 400;
            LoadSprite(GlobalPaths.ProjectileSprites + "pistolprojectile.png");
            RigidBody.Resize(new Physics.Vector(10, 10));
        }
    }
}
