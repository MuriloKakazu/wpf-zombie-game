using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game.Interfaces
{
    public interface ISerializableEnemy
    {
        /// <summary>
        /// Retorna o nome do inimigo
        /// </summary>
        string Name { get; set; }
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
        /// Nome do arquivo de sprite do projétil
        /// </summary>
        string SpriteFileName { get; set; }
        /// <summary>
        /// Massa do projétil
        /// </summary>
        float Mass { get; set; }
        /// <summary>
        /// Multiplicador do módulo da velocidade
        /// </summary>
        float SpeedMultiplier { get; set; }
        /// <summary>
        /// Tamanho do projétil
        /// </summary>
        Vector Size { get; set; }

        /// <summary>
        /// Monta uma instância do objeto Enemy
        /// </summary>
        /// <returns>Enemy</returns>
        Enemy Mount();
    }
}
