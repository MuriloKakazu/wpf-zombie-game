using ZombieGame.Game.Enums;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game.Prefabs.Weapons
{
    public sealed class HeavyMG : Weapon
    {
        public HeavyMG() : base(WeaponTypes.HeavyMG.ToString(), WeaponTypes.HeavyMG, ProjectileTypes.HeavyMG)
        {
            FireRate = WDB.HMGfr;
            ReloadTime = WDB.HMGrt;
            Ammo = WDB.HMGammo;
        }
    }
}

