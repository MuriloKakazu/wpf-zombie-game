using System.Collections.Generic;

namespace ZombieGame.Game
{
    public static class Database
    {
        /// <summary>
        /// Lista de todas as armas disponíveis no jogo
        /// </summary>
        public static List<Weapon> Weapons;
        /// <summary>
        /// Lista de todos os projéteis disponíveis no jogo
        /// </summary>
        public static List<Projectile> Projectiles;
        /// <summary>
        /// Lista de todos os itens disponíveis no jogo
        /// </summary>
        public static List<Item> Items;
    }
}
