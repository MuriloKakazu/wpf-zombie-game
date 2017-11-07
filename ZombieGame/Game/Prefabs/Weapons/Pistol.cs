using ZombieGame.Game.Enums;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game.Prefabs.Weapons
{
    public sealed class Pistol : Weapon
    {
        public Pistol() : base(WeaponTypes.Pistol.ToString(), WeaponTypes.Pistol, ProjectileTypes.Pistol)
        {
            FireRate = WDB.pistolFR;
            ReloadTime = WDB.pistolRT;
            Ammo = WDB.pistolAmmo;
        }
    }
}
