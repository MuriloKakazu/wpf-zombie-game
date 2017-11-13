using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game.Interfaces
{
    public interface ISerializableEnemy : ISerializableEntity
    {
        /// <summary>
        /// Retorna o tipo de inimigo
        /// </summary>
        EnemyTypes Type { get; set; }
        /// <summary>
        /// Retorna a pontuação que o inimigo dará ao morrer
        /// </summary>
        float DeathPoints { get; set; }
        /// <summary>
        /// Retorna o dinheiro que o inimigo dará ao morrer
        /// </summary>
        float MoneyDrop { get; set; }
        /// <summary>
        /// Multiplicador do módulo da velocidade
        /// </summary>
        float SpeedMultiplier { get; set; }

        /// <summary>
        /// Monta uma instância do objeto Enemy
        /// </summary>
        /// <returns>Enemy</returns>
        Enemy Mount();
    }
}
