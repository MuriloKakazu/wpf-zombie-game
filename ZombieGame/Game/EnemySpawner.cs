using System;
using System.Windows;
using ZombieGame.Extensions;
using ZombieGame.Game.Entities;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public static class EnemySpawner
    {
        /// <summary>
        /// Gerador de números randômicos
        /// </summary>
        static Random R = new Random();
        const int MaxEnemiesAllowed = 50;
        const int MinEnemiesAllowed = 5;
        static int spawnTarget = MinEnemiesAllowed;
        public static int CurrentEnemySpawnTarget { get { return spawnTarget; } set { spawnTarget = value; if (spawnTarget > MaxEnemiesAllowed) spawnTarget = MaxEnemiesAllowed; } }

        public static void Setup()
        {
            Time.LowFrequencyTimer.Elapsed += LowFrequencyTimer_Elapsed;
        }

        private static void LowFrequencyTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate { SpawnRandomEnemy(); });
        }

        /// <summary>
        /// Gera um novo inimigo do tipo normal
        /// </summary>
        public static void SpawnRandomEnemy()
        {
            for (int i = 0; i < 3 && Enemy.Instances.Length < CurrentEnemySpawnTarget; i++)
            {
                Enemy e = Database.Enemies.PickAny().Mount();
                e.RigidBody.SetPosition(GetRandomPositionOffCamera(e));
            }
        }

        /// <summary>
        /// Retorna uma posição aleatória dentro da janela de jogo
        /// </summary>
        /// <returns></returns>
        private static Physics.Vector GetRandomPositionOffCamera(Enemy e)
        {
            var scene = GameMaster.CurrentScene;
            Physics.Vector v = new Physics.Vector(R.Next((int)scene.RenderPosition.X, (int)scene.Size.X), R.Next((int)scene.RenderPosition.Y, (int)scene.Size.Y));
            while (new Rect(v.X - 500, v.Y - 500, 500, 500).IntersectsWith(GameMaster.Camera.RigidBody.Bounds))
            {
                v = new Physics.Vector(R.Next((int)scene.RenderPosition.X, (int)scene.Size.X), R.Next((int)scene.RenderPosition.Y, (int)scene.Size.Y));
            }
                return v;

        }
    }
}
