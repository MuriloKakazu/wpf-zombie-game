using System.Collections.Generic;
using System.IO;
using ZombieGame.Game.Interfaces;
using ZombieGame.Game.Serializable;
using ZombieGame.IO.Serialization;
using ZombieGame.Physics;

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
        /// Lista de todos os cenários disponíveis no jogo
        /// </summary>
        public static List<Scene> Scenes;

        public static void Setup()
        {
            Weapons = Weapons.LoadFrom(IO.GlobalPaths.DB + "weapons.db");
            Projectiles = Projectiles.LoadFrom(IO.GlobalPaths.DB + "projectiles.db");
            Scenes = new List<Scene>();

            string[] files = Directory.GetFiles(IO.GlobalPaths.Scenes);
            foreach (var f in files)
                Scenes.Add(new Scene().LoadFrom(f));
        }
    }
}
