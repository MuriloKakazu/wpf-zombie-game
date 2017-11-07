using ZombieGame.Game.Enums;
using ZombieGame.IO;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game.Prefabs.Projectiles
{
    public sealed class HeavyMGProjectile : Projectile
    {
        public HeavyMGProjectile() : base(ProjectileTypes.HeavyMG)
        {
            HitDamage = PDB.HMGdmg;
            SpeedMagnitude = PDB.HMGspd;
            KnockbackMagnitude = 15;
            LoadSprite(GlobalPaths.ProjectileSprites + "pistolprojectile.png");
            RigidBody.Resize(new Physics.Vector(10, 10));
        }
    }
}
