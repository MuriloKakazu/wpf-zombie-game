using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame.Physics.Extensions
{
    /// <summary>
    /// Infelizmente, não é possível criar métodos de extensão para a classe Math, pois ela é estática.
    /// </summary>
    public static class MathExtension
    {
        /// <summary>
        /// Converte o valor do ângulo de radianos para graus
        /// </summary>
        /// <param name="angle">Valor em radianos</param>
        /// <returns>Valor em graus</returns>
        public static float RadiansToDegrees(float angle)
        {
            return angle * (180f / (float)Math.PI);
        }

        /// <summary>
        /// Converte o valor do ângulo de graus para radianos
        /// </summary>
        /// <param name="angle">Valor em graus</param>
        /// <returns>Valor em radianos</returns>
        public static float DegreesToRadians(float angle)
        {
            return (float)Math.PI * angle / 180f;
        }
    }
}
