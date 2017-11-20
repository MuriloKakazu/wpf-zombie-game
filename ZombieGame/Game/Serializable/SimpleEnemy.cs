using System;
using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Interfaces;
using ZombieGame.Physics;

namespace ZombieGame.Game.Serializable
{
    [Serializable]
    public class SimpleEnemy : ISerializableEnemy
    {
        /// <summary>
        /// Retorna o nome do inimigo
        /// </summary>
        public string Name { get; set; }
        public float Health { get; set; }
        /// <summary>
        /// Retorna o tipo de inimigo
        /// </summary>
        public EnemyType Type { get; set; }
        /// <summary>
        /// Retorna a pontuação que o inimigo dará ao morrer
        /// </summary>
        public float DeathPoints { get; set; }
        /// <summary>
        /// Retorna o dinheiro que o inimigo dará ao morrer
        /// </summary>
        public float MoneyDrop { get; set; }
        /// <summary>
        /// Nome do arquivo de sprite do projétil
        /// </summary>
        public string SpriteFileName { get; set; }
        /// <summary>
        /// Massa do projétil
        /// </summary>
        public float Mass { get; set; }
        /// <summary>
        /// Multiplicador do módulo da velocidade
        /// </summary>
        public float SpeedMultiplier { get; set; }
        /// <summary>
        /// Tamanho do projétil
        /// </summary>
        public Vector Size { get; set; }
        public string DeathSFXKey { get; set; }
        public string HitSFXKey { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public SimpleEnemy() { }

        /// <summary>
        /// Monta uma instância do objeto Enemy
        /// </summary>
        /// <returns>Enemy</returns>
        public Enemy Mount()
        {
            return Enemy.Mount(this);
        }
    }
}
