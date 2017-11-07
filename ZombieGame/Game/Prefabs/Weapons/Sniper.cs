using ZombieGame.Game.Enums;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game.Prefabs.Weapons
{
    public sealed class Sniper : Weapon
    {
        public Sniper() : base(WeaponTypes.SniperRifle.ToString(), WeaponTypes.SniperRifle, ProjectileTypes.Sniper)
        {
            FireRate = WDB.sniperFR;
            ReloadTime = WDB.sniperRT;
            Ammo = WDB.sniperAmmo;
        }
    }
}