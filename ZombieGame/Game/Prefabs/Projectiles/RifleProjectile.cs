using ZombieGame.Game.Enums;
using ZombieGame.IO;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game.Prefabs.Projectiles
{
    public sealed class RifleProjectile : Projectile
    {
        public RifleProjectile() : base(ProjectileTypes.Rifle)
        {
            HitDamage = PDB.rifleDmg;
            SpeedMagnitude = PDB.rifleSpd;
            KnockbackMagnitude = 15;
            LoadSprite(GlobalPaths.ProjectileSprites + "pistolprojectile.png");
            RigidBody.Resize(new Physics.Vector(10, 10));
        }
    }
}
