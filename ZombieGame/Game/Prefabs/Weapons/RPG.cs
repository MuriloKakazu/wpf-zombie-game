using ZombieGame.Game.Enums;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game.Prefabs.Weapons
{
    public sealed class RPG : Weapon
    {
        public RPG() : base(WeaponTypes.Explosive.ToString(), WeaponTypes.Explosive, ProjectileTypes.Missile)
        {
            FireRate = WDB.missileFR;
            ReloadTime = WDB.missileRT;
            Ammo = WDB.missileAmmo;
        }
    }
}
