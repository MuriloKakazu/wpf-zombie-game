using System;
using System.Collections.Generic;
using ZombieGame.Game.Serializable;
using System.Linq;

namespace ZombieGame.Game
{
    public static class Store
    {
        /// O list com todas as armas à venda
        /// </summary>
        public static List<SimpleWeapon> SellingWeapons;
        /// <summary>
        /// O list com todos os projéteis à venda
        /// </summary>
        public static List<SimpleProjectile> SellingProjectiles;

        /// <summary>
        /// O método utilizado ao comprar uma arma
        /// </summary>
        /// <param name="itemID">O ID da arma</param>
        public static void BuyWeapon(int itemID)
        {
            SimpleWeapon w = FindWeapon(itemID);
            if (w == null)
                throw new Exception("Não é possível comprar a arma com ID: " + itemID + ".\nEsse ID não existe!");
            if (w.Price > GameMaster.Money)
                throw new Exception("Os jogadores não possuem dinheiro para comprar esta arma.\nEla não deveria estar disponível!");
            if (w.Sold)
                throw new Exception("Esta arma já foi vendida.\nEla não deveria estar à venda!");

            FindWeapon(itemID).Sold = true;
            GameMaster.Money -= w.Price;
        }

        /// <summary>
        /// O método utilizado ao comprar um projétil
        /// </summary>
        /// <param name="itemID">O ID do projétil</param>
        public static void BuyProjectile(int itemID)
        {
            SimpleProjectile p = FindProjectile(itemID);
            if (p == null)
                throw new Exception("Não é possível comprar a arma com ID: " + itemID + ".\nEsse ID não existe!");
            if (p.Price > GameMaster.Money)
                throw new Exception("Os jogadores não possuem dinheiro para comprar esta arma.\nEla não deveria estar disponível!");
            if (p.Sold)
                throw new Exception("Este projétil já foi vendido.\nEle não deveria estar à venda!");

            FindProjectile(itemID).Sold = true;
            GameMaster.Money -= p.Price;
        }

        /// <summary>
        /// O método utilizado para retornar um projétil da lista
        /// </summary>
        /// <param name="itemID">O ID do projétil desejado</param>
        /// <returns>O projétil encontrado ou nulo</returns>
        private static SimpleProjectile FindProjectile(int itemID)
        {
            foreach (var item in SellingProjectiles)
                if (item.ItemID == itemID)
                    return item;
            Console.WriteLine("There's no item with the ID: " + itemID);
            return null;
        }

        /// <summary>
        /// O método utilizado para retornar uma arma da lista
        /// </summary>
        /// <param name="itemID">O ID da arma desejada</param>
        /// <returns>A arma se encontrada ou nulo</returns>
        private static SimpleWeapon FindWeapon(int itemID)
        {
            foreach (var item in SellingWeapons)
                if (item.ItemID == itemID)
                    return item;
            Console.WriteLine("There's no item with the ID: " + itemID);
            return null;
        }

        /// <summary>
        /// O método para definir as listas de itens
        /// <para>É uma espécie de construtor da classe</para>
        /// </summary>
        public static void SetSellingItems()
        {
            SellingWeapons = Database.Weapons.OrderBy(x => x.Price).ToList();
            SellingProjectiles = Database.Projectiles.OrderBy(x => x.Price).ToList();
        }

        /// <summary>
        /// Método responsável por zerar os itens comprados na loja.
        /// </summary>
        public static void ResetStore()
        {

        }
    }
}