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
        #region Properties
        /// <summary>
        /// Retorna o dano da arma por projétil
        /// </summary>
        public int Damage { get; set; }
        /// <summary>
        /// Retorna a taxa de disparo de projéteis da arma por minuto
        /// </summary>
        public float FireRate { get; set; }

        /// <summary>
        /// Retorna a quantidade máxima de munição
        /// </summary>
        public int MaxAmmo { get; set; }

        /// <summary>
        /// Retorna a quantidade atual de munição
        /// </summary>
        public int CurAmmo { get; set; }

        /// <summary>
        /// Retorna a velocidade da arma
        /// </summary>
        public float BulletVelocity { get; set; }

        /// <summary>
        /// Retorna o tipo da arma
        /// </summary>
        public WeaponTypes Type { get; set; }
	    #endregion
    }
}
