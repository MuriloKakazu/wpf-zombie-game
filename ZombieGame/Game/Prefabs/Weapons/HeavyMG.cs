using ZombieGame.Game.Enums;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game.Prefabs.Weapons
{
    public sealed class HeavyMG : Weapon
    {
        public HeavyMG() : base(WeaponTypes.HMG.ToString(), WeaponTypes.HMG, ProjectileTypes.Buckshot)
        {
            FireRate = WDB.HMGfr;
            ReloadTime = WDB.HMGrt;
            Ammo = WDB.HMGammo;
        }
    }
}

