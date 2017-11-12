using ZombieGame.Game.Enums;

namespace ZombieGame.Game.Interfaces
{
    public interface ISerializableWeapon
    {
        /// <summary>
        /// Nome da arma
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Taxa de disparo de projéteis da arma por minuto
        /// </summary>
        float FireRate { get; set; }
        /// <summary>
        /// Quantia de munição da arma
        /// </summary>
        int Ammo { get; set; }
        /// <summary>
        /// O tempo para recarregar a arma
        /// </summary>
        float ReloadTime { get; set; }
        /// <summary>
        /// Tipo da arma
        /// </summary>
        WeaponTypes Type { get; set; }
        /// <summary>
        /// Tipos de projéteis aceitos pela arma
        /// </summary>
        ProjectileTypes[] AcceptedProjectileTypes { get; set; }

        /// <summary>
        /// Monta uma instância do objeto Weapon
        /// </summary>
        /// <returns>Weapon</returns>
        Weapon Mount();
    }
}
