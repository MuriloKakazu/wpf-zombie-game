using ZombieGame.Game.Enums;
using ZombieGame.IO;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game.Prefabs.Projectiles
{
    public sealed class SniperProjectile : Projectile
    {
        public SniperProjectile() : base(ProjectileTypes.HollowPoint)
        {
            /*HitDamage = PDB.sniperDmg;
            SpeedMagnitude = PDB.sniperSpd;*/
            KnockbackMagnitude = 100;
            LoadSprite(GlobalPaths.ProjectileSprites + "pistolprojectile.png");
            RigidBody.Resize(new Physics.Vector(10, 10));
        }
    }
}
