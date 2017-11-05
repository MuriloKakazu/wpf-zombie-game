using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Prefabs.Characters;
using ZombieGame.IO;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public static class EnemySpawner
    {
        static Random R = new Random();

        public static void SpawnRandomEnemy()
        {
            var r = R.Next(0, 2);
            if (r == 0)
                SpawnRunner();
            else if (r == 1)
                SpawnZombie();
            else if (r == 2)
                SpawnTanker();
        }

        public static void SpawnTanker()
        {
            var e = new Tanker();
        }

        public static void SpawnZombie()
        {
            var e = new Zombie();
        }

        public static void SpawnRunner()
        {
            var e = new Runner();
        }

        public static void SpawnBoss()
        {
            var e = new Boss();
        }

        public static Vector GetRandomPositionOffscreen()
        {
            ///
            return Vector.Zero;
        }
    }
}
