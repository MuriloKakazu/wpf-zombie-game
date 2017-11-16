using System.Collections.Generic;
using System.IO;
using ZombieGame.Audio;
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
        public static List<SimpleWeapon> Weapons { get; set; }
        /// <summary>
        /// Lista de todos os projéteis disponíveis no jogo
        /// </summary>
        public static List<SimpleProjectile> Projectiles { get; set; }
        /// <summary>
        /// Lista de todos os personagens inimigos disponíveis no jogo
        /// </summary>
        public static List<SimpleEnemy> Enemies { get; set; }
        /// <summary>
        /// Lista de todos os cenários disponíveis no jogo
        /// </summary>
        public static List<Scene> Scenes { get; set; }

        public static void Setup()
        {
            Weapons = Weapons.LoadFrom(IO.GlobalPaths.DB + "weapons.db");
            Projectiles = Projectiles.LoadFrom(IO.GlobalPaths.DB + "projectiles.db");
            Enemies = Enemies.LoadFrom(IO.GlobalPaths.DB + "enemies.db");
            Scenes = new List<Scene>();

            string[] files = Directory.GetFiles(IO.GlobalPaths.Scenes);
            foreach (var f in files)
                Scenes.Add(new Scene().LoadFrom(f));
        }
    }
}
