using System;
using System.Windows;
using ZombieGame.Extensions;
using ZombieGame.Game.Entities;
using ZombieGame.Game.Prefabs.Entities;
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
        /// Gera um novo inimigo do tipo normal
        /// </summary>
        public static void SpawnRandomEnemy()
        {
            for (int i = 0; i < 10; i++)
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
            Physics.Vector v = new Physics.Vector(R.Next(-4000, 4000), R.Next(-4000, 4000));
            while (new Rect(v.X - 1000, v.Y - 1000, 1500, 1500).IntersectsWith(GameMaster.Camera.RigidBody.Bounds))
            {
                v = new Physics.Vector(R.Next(-4000, 4000), R.Next(-4000, 4000));
            }
                return v;

        }
    }
}
