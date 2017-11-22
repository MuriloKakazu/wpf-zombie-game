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
        /// <summary>
        /// Número máximo de inimigos
        /// </summary>
        static int MaxEnemiesAllowed = (int)(20 * GameMaster.DifficultyBonus);
        /// <summary>
        /// Número mínimo de inimigos
        /// </summary>
        const int MinEnemiesAllowed = 5;
        /// <summary>
        /// Número de inimigos que o jogo quer aparecendo na tela
        /// </summary>
        private static int WantedEnemyCount { get { return MinEnemiesAllowed + (int)Math.Round(GameMaster.Score / 1000, 0, MidpointRounding.AwayFromZero); } }
        /// <summary>
        /// Número máximo de inimigos que deverão aparecer na tela atualmente
        /// </summary>
        public static int CurrentEnemySpawnTarget { get { if (WantedEnemyCount > MaxEnemiesAllowed) return MaxEnemiesAllowed; else return WantedEnemyCount; } }

        /// <summary>
        /// Prepara o Spawner de inimigos
        /// </summary>
        public static void Setup()
        {
            Time.LowFrequencyTimer.Elapsed += LowFrequencyTimer_Elapsed;
        }

        /// <summary>
        /// Evento que atualiza algumas informações
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            Physics.Vector v = new Physics.Vector(R.Next((int)scene.RenderPosition.X, (int)scene.Size.X), -R.Next((int)scene.RenderPosition.Y, (int)scene.Size.Y));
            while (new Rect(v.X - 500, v.Y - 500, 1000, 1000).IntersectsWith(GameMaster.Camera.RigidBody.Bounds))
            {
                v = new Physics.Vector(R.Next((int)scene.RenderPosition.X, (int)scene.Size.X), -R.Next((int)scene.RenderPosition.Y, (int)scene.Size.Y));
            }
                return v;

        }
    }
}
