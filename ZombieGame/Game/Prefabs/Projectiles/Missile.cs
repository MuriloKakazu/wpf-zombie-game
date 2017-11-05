using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.IO;

namespace ZombieGame.Game.Prefabs.Projectiles
{
    public sealed class Missile : Projectile
    {
        public Missile() : base(ProjectileTypes.Missile)
        {
            this.HitDamage = 1;
            this.SpeedMagnitude = 500;
            this.IsExplosive = true;
            LoadSprite(GlobalPaths.ProjectileSprites + "pistolprojectile.png");
            RigidBody.Resize(new Physics.Vector(10, 10));
        }
    }
}
