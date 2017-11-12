using System.Collections.Generic;
using ZombieGame.Game.Interfaces;
using ZombieGame.Game.Serializable;

namespace ZombieGame.Game
{
    public static class Database
    {
        /// <summary>
        /// Lista de todas as armas disponíveis no jogo
        /// </summary>
        public static List<SimpleWeapon> Weapons;
        /// <summary>
        /// Lista de todos os projéteis disponíveis no jogo
        /// </summary>
        public static List<SimpleProjectile> Projectiles;
        /// <summary>
        /// Lista de todos os personagens inimigos disponíveis no jogo
        /// </summary>
        public static List<SimpleEnemy> Enemies;
        /// <summary>
        /// Lista de todos os itens disponíveis no jogo
        /// </summary>
        //public static List<Item> Items;
    }
}
