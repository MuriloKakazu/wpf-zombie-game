using System;
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
        public static void SpawnRandomNonBossEnemy()
        {
            
        }

        /// <summary>
        /// Retorna uma posição aleatória dentro da janela de jogo
        /// </summary>
        /// <returns></returns>
        private static Vector GetRandomPosition()
        {
            return new Vector(500 + R.Next(-500, 500), -500 + R.Next(-200, 200));
        }
    }
}
