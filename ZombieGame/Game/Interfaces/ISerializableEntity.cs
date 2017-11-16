using ZombieGame.Physics;

namespace ZombieGame.Game.Interfaces
{
    public interface ISerializableEntity
    {
        /// <summary>
        /// Retorna o nome da entidade
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Massa do projétil
        /// </summary>
        float Mass { get; set; }
        /// <summary>
        /// Tamanho do projétil
        /// </summary>
        Vector Size { get; set; }
        /// <summary>
        /// Nome do arquivo de sprite do projétil
        /// </summary>
        string SpriteFileName { get; set; }
    }
}
