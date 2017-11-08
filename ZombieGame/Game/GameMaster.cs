using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using ZombieGame.Game.Prefabs.DataBase;
using ZombieGame.Game.Prefabs.OtherEntities;
using ZombieGame.IO.Serialization;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public static class GameMaster
    {
        /// <summary>
        /// Câmera do jogo
        /// </summary>
        public static Camera Camera { get; private set; }
        /// <summary>
        /// Jogador 1
        /// </summary>
        public static Player Player1 { get; private set; }
        /// <summary>
        /// Jogador 2
        /// </summary>
        public static Player Player2 { get; private set; }
        /// <summary>
        /// Retorna o cenário atual do jogo
        /// </summary>
        public static Scene CurrentScene { get; private set; }
        /// <summary>
        /// Retorna todos os cenários de jogo disponíveis
        /// </summary>
        public static Scene[] Scenes { get; private set; }
        /// <summary>
        /// Retorna os dados de todos os projéteis
        /// </summary>
        public static ProjectilesDB PDB { get; private set; }

        /// <summary>
        /// Retorna os dados de todas as armas
        /// </summary>
        public static WeaponsDB WDB { get; private set; }

        /// <summary>
        /// Define as configurações iniciais do jogo
        /// </summary>
        public static void Setup()
        {
            DataBase.Weapons.LoadFrom(IO.GlobalPaths.DB + "weapons.db");

            
            DataBase.Items = new List<Item>();
            DataBase.Items.Add(new Item("x", Enums.ItemType.Weapon));
            DataBase.Items.SaveTo(IO.GlobalPaths.DB + "items.db");

            /*
            Store.SetSellingItems();
            Time.Setup();
            Camera = new Camera();
            Player1 = new Player(1, "Player1");
            Player2 = new Player(2, "Player2");
            Player1.Character.RigidBody.UseRotation = true;
            Player2.Character.RigidBody.UseRotation = true;
            Player1.Character.RigidBody.SetPosition(new Vector(100, 0));
            Player2.Character.RigidBody.SetPosition(new Vector(500, 0));
            Player1.Character.RigidBody.Resize(new Vector(50, 50));
            Player2.Character.RigidBody.Resize(new Vector(50, 50));

            for (int i = 0; i < 1; i++)
                EnemySpawner.SpawnZombie();*/
        }

        public static Player GetPlayer(int number)
        {
            if (number == 1)
                return Player1;
            else if (number == 2)
                return Player2;
            return null;
        }
    }
}
