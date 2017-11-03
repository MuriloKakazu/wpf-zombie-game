using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public class Weapon
    {
        /// <summary>
        /// Retorna o dano da arma por projétil
        /// </summary>
        public float Damage { get; set; }
        /// <summary>
        /// Retorna a taxa de disparo de projéteis da arma por minuto
        /// </summary>
        public float FireRate { get; set; }
        /// <summary>
        /// Retorna o tipo da arma
        /// </summary>
        public WeaponTypes Type { get; set; }
    }
}
