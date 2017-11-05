using ZombieGame.Game.Enums;
using ZombieGame.IO;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game.Prefabs.Projectiles
{
    public sealed class Missile : Projectile
    {
        public Missile() : base(ProjectileTypes.Missile)
        {
            HitDamage = PDB.missileDmg;
            SpeedMagnitude = PDB.missileSpd;
            LoadSprite(GlobalPaths.ProjectileSprites + "pistolprojectile.png");
            RigidBody.Resize(new Physics.Vector(10, 10));
        }
    }
}
