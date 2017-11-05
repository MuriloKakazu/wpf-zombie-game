using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;

namespace ZombieGame.Game
{
    public class Item
    {
        /// <summary>
        /// The number of instanced items.
        /// </summary>
        public static int NumberOfItens = 0;

        /// <summary>
        /// The name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Return the price to buy the item in the store.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// Return the owner of the item.
        /// <para>The item just have an owner if the variable 'sold' is true</para>
        /// </summary>
        public Character Owner { get; set; } //If null, dont have an owner

        /// <summary>
        /// Return type of the item.
        /// </summary>
        public ItemType Type { get; set; }

        /// <summary>
        /// Return if the item was sold to someone.
        /// </summary>
        public bool Sold { get; set; }

        /// <summary>
        /// Return the ID of the item.
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="price">The price</param>
        /// <param name="type">The type</param>
        public Item(string name, int price, ItemType type)
        {
            Name = name;
            Price = price;
            Type = type;
            ItemID = ++NumberOfItens;
        }
    }
}
