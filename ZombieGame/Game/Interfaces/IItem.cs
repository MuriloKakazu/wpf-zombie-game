using ZombieGame.Game.Enums;

namespace ZombieGame.Game.Interfaces
{
    public interface IItem
    {
        /// <summary>
        /// Retorna o preço do item na loja
        /// </summary>
        int Price { get; set; }
        /// <summary>
        /// Retorna o tipo do item
        /// </summary>
        ItemType Type { get; }
        /// <summary>
        /// Retorna se já foi vendido ou não. Ou seja, se o personagem possui o item.
        /// </summary>
        bool Sold { get; set; }
        /// <summary>
        /// Retorna o ID do item. 
        /// </summary>
        int ItemID { get; set; }
    }
}