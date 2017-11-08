using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using static ZombieGame.Game.Prefabs.DataBase.ItemsDB;

namespace ZombieGame.Game.Prefabs.Items
{
    public class AssaultRifle : Item
    {
        public AssaultRifle() : base(WeaponTypes.AssaultRifle.ToString(), ItemType.Weapon)
        {
            Price = riflePrice;
            
        }
    }
}
