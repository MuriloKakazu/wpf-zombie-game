using ZombieGame.Game.Enums;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game.Prefabs.Weapons
{
    public sealed class Rifle : Weapon
    {
        public Rifle() : base(WeaponTypes.AssaultRifle.ToString(), WeaponTypes.AssaultRifle, ProjectileTypes.Arrow)
        {
            FireRate = WDB.rifleFR;
            ReloadTime = WDB.rifleRT;
            Ammo = WDB.rifleAmmo;
        }
    }
}

