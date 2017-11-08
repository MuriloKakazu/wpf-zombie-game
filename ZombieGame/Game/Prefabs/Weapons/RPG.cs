using ZombieGame.Game.Enums;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game.Prefabs.Weapons
{
    public sealed class RPG : Weapon
    {
        public RPG() : base(WeaponTypes.RocketLauncher.ToString(), WeaponTypes.RocketLauncher, ProjectileTypes.Explosive)
        {
            FireRate = WDB.missileFR;
            ReloadTime = WDB.missileRT;
            Ammo = WDB.missileAmmo;
        }
    }
}
