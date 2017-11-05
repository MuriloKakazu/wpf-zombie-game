using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame.Game
{
    public static class Store
    {
        /// <summary>
        /// The list of itens that are selling in the store, or already have been sold.
        /// </summary>
        public static List<Item> SellingItens = new List<Item>();
    }
}