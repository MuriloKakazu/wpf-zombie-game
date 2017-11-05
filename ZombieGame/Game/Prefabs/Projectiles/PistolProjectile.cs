using ZombieGame.Game.Enums;
using ZombieGame.IO;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game.Prefabs.Projectiles
{
    public sealed class PistolProjectile : Projectile
    {
        public PistolProjectile() : base(ProjectileTypes.Pistol)
        {
            HitDamage = PDB.pistolDmg;
            SpeedMagnitude = PDB.pistolSpd;
            LoadSprite(GlobalPaths.ProjectileSprites + "pistolprojectile.png");
            RigidBody.Resize(new Physics.Vector(10, 10));
        }
    }
}
