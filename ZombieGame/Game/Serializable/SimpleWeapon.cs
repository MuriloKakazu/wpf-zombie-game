using System;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Interfaces;
using ZombieGame.Physics;

namespace ZombieGame.Game.Serializable
{
    [Serializable]
    public class SimpleWeapon : ISerializableWeapon, IItem
    {
        /// <summary>
        /// Nome da arma
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Taxa de disparo de projéteis da arma por minuto
        /// </summary>
        public float FireRate { get; set; }
        /// <summary>
        /// Quantia de munição da arma
        /// </summary>
        public int MagSize { get; set; }
        /// <summary>
        /// O tempo para recarregar a arma
        /// </summary>
        public float ReloadTime { get; set; }
        /// <summary>
        /// Tipo da arma
        /// </summary>
        public WeaponTypes Type { get; set; }
        /// <summary>
        /// Tipos de projéteis aceitos pela arma
        /// </summary>
        public ProjectileTypes[] AcceptedProjectileTypes { get; set; }
        public float Mass { get; set; }
        public Vector Size { get; set; }
        public string SpriteFileName { get; set; }
        /// <summary>
        /// Preço do item
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// Tipo de item
        /// </summary>
        ItemType IItem.Type { get { return ItemType.Weapon; } }
        /// <summary>
        /// Retorna se o item foi adquirido
        /// </summary>
        public bool Sold { get; set; }
        /// <summary>
        /// ID do item
        /// </summary>
        public int ItemID { get; set; }
        public string SoundFXKey { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public SimpleWeapon() { }

        /// <summary>
        /// Monta uma instância do objeto Weapon
        /// </summary>
        /// <returns>Weapon</returns>
        public Weapon Mount()
        {
            return Weapon.Mount(this);
        }
    }
}
