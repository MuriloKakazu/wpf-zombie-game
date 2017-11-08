using ZombieGame.Game.Enums;
using ZombieGame.IO;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game.Prefabs.Projectiles
{
    public sealed class Missile : Projectile
    {
        public Missile() : base(ProjectileTypes.Explosive)
        {
            /*HitDamage = PDB.missileDmg;
            SpeedMagnitude = PDB.missileSpd;*/
            IsExplosive = true;
            KnockbackMagnitude = 100;
            LoadSprite(GlobalPaths.ProjectileSprites + "missile.png");
            RigidBody.Resize(new Physics.Vector(40, 40));
        }
    }
}
