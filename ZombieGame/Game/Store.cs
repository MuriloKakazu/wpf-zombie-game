using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Entities;

namespace ZombieGame.Game
{
    public static class Store
    {
        /// <summary>
        /// The list of itens that are selling in the store, or already have been sold.
        /// </summary>
        public static List<Item> SellingItems = new List<Item>();

        public static void BuyItem(int itemID, Character character)
        {
            
        }

        private static Item FindItem(int itemID)
        {
            return null;
        }

        public static void SetSellingItems()
        {

        }
    }
}