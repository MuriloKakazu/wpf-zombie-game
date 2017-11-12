using System;
using ZombieGame.Physics.Extensions;

namespace ZombieGame.Physics
{
    [Serializable]
    public class Vector
    {
        #region Properties
        /// <summary>
        /// Vetor (0, 0, 0)
        /// </summary>
        public static Vector Zero { get { return new Vector(0, 0, 0); } }
        /// <summary>
        /// Vetor (0, 1, 0)
        /// </summary>
        public static Vector Up { get { return new Vector(0, 1, 0); } }
        /// <summary>
        /// Vetor (0, -1, 0)
        /// </summary>
        public static Vector Down { get { return new Vector(0, -1, 0); } }
        /// <summary>
        /// Vetor (-1, 0, 0)
        /// </summary>
        public static Vector Left { get { return new Vector(-1, 0, 0); } }
        /// <summary>
        /// Vetor (1, 0, 0)
        /// </summary>
        public static Vector Right { get { return new Vector(1, 0, 0); } }
        /// <summary>
        /// Vetor (10000, 10000)
        /// </summary>
        public static Vector OffScreen { get { return new Vector(10000, 10000); } }
        /// <summary>
        /// Vetor (1274, 691)
        /// </summary>
        public static Vector WindowSize { get { return new Vector(1274, 691); } }

        /// <summary>
        /// Retorna o valor no eixo X
        /// </summary>
        public float X { get; set; }
        /// <summary>
        /// Retorna o valor no eixo Y
        /// </summary>
        public float Y { get; set; }
        /// <summary>
        /// Retorna o valor no eixo Z
        /// </summary>
        public float Z { get; set; }
        /// <summary>
        /// Retorna o módulo do vetor
        /// </summary>
        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt(Math.Pow(X, 2) +
                                        Math.Pow(Y, 2) +
                                        Math.Pow(Z, 2));
            }
        }
        /// <summary>
        /// Retorna a parte unitária do vetor
        /// </summary>
        public Vector Normalized
        {
            get
            {
                var mag = Magnitude;

                if (Magnitude == 0)
                    return Vector.Zero;
                else
                    return new Vector(X / mag, Y / mag, Z / mag);
            }
        }
        /// <summary>
        /// Retorna o vetor oposto
        /// </summary>
        public Vector Opposite
        {
            get { return -this; }
        }
        /// <summary>
        /// Retorna se o vetor tem módulo igual a 1
        /// </summary>
        public bool IsNormalized { get { return Math.Round(Magnitude, 2) == 1; } } // 0.995 e 1.005 são considerados como 1
        #endregion

        #region Operators
        /// <summary>
        /// Retorna o produto de um vetor por um número
        /// </summary>
        /// <param name="v">Vetor</param>
        /// <param name="n">Número</param>
        /// <returns>Vetor resultante</returns>
        public static Vector operator *(Vector v, float n)
        {
            return new Vector(v.X * n, v.Y * n, v.Z * n);
        }
        /// <summary>
        /// Retorna o produto entre dois vetores
        /// </summary>
        /// <param name="v1">Vetor A</param>
        /// <param name="v2">Vetor B</param>
        /// <returns>Vetor resultante</returns>
        public static Vector operator *(Vector v1, Vector v2)
        {
            return new Vector(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }
        /// <summary>
        /// Retorna a divisão de um vetor por um número
        /// </summary>
        /// <param name="v">Vetor</param>
        /// <param name="n">Número</param>
        /// <returns>Vetor resultante</returns>
        public static Vector operator /(Vector v, float n)
        {
            return new Vector(v.X / n, v.Y / n, v.Z / n);
        }
        /// <summary>
        /// Retorna a soma entre dois vetores
        /// </summary>
        /// <param name="v1">Vetor A</param>
        /// <param name="v2">Vetor B</param>
        /// <returns>Vetor resultante</returns>
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        /// <summary>
        /// Retorna a subtração entre dois vetores
        /// </summary>
        /// <param name="v1">Vetor A</param>
        /// <param name="v2">Vetor B</param>
        /// <returns>Vetor resultante</returns>
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        /// <summary>
        /// Retorna o vetor oposto
        /// </summary>
        /// <param name="v1">Vetor original</param>
        /// <returns>Vetor resultante</returns>
        public static Vector operator -(Vector v1)
        {
            return v1 * -1;
        }
        // Obsolete code
        ///// <summary>
        ///// Retorna um booleano afirmando se o módulo de A é maior do que o módulo de B
        ///// </summary>
        ///// <param name="v1">Vetor A</param>
        ///// <param name="v2">Vetor B</param>
        ///// <returns>Bool</returns>
        //public static bool operator >(Vector v1, Vector v2)
        //{
        //    return Math.Round(v1.Magnitude, 2) > Math.Round(v2.Magnitude, 2); // 0.995 e 1.005 são considerados como 1
        //}
        ///// <summary>
        ///// Retorna um booleano afirmando se o módulo de A é menor do que o módulo de B
        ///// </summary>
        ///// <param name="v1">Vetor A</param>
        ///// <param name="v2">Vetor B</param>
        ///// <returns>Bool</returns>
        //public static bool operator <(Vector v1, Vector v2)
        //{
        //    return Math.Round(v1.Magnitude, 2) < Math.Round(v2.Magnitude, 2); // 0.995 e 1.005 são considerados como 1
        //}
        ///// <summary>
        ///// Retorna um booleano afirmando se o módulo de A é maior do que ou igual ao módulo de B
        ///// </summary>
        ///// <param name="v1">Vetor A</param>
        ///// <param name="v2">Vetor B</param>
        ///// <returns>Bool</returns>
        //public static bool operator >=(Vector v1, Vector v2)
        //{
        //    return Math.Round(v1.Magnitude, 2) >= Math.Round(v2.Magnitude, 2); // 0.995 e 1.005 são considerados como 1
        //}
        ///// <summary>
        ///// Retorna um booleano afirmando se o módulo de A é menor do que ou igual ao módulo de B
        ///// </summary>
        ///// <param name="v1">Vetor A</param>
        ///// <param name="v2">Vetor B</param>
        ///// <returns>Bool</returns>
        //public static bool operator <=(Vector v1, Vector v2)
        //{
        //    return Math.Round(v1.Magnitude, 2) <= Math.Round(v2.Magnitude, 2); // 0.995 e 1.005 são considerados como 1
        //}
        ///// <summary>
        ///// Retorna um booleano afirmando se os módulos dos vetores são iguais
        ///// </summary>
        ///// <param name="v1">Vetor A</param>
        ///// <param name="v2">Vetor B</param>
        ///// <returns>Bool</returns>
        //public static bool operator ==(Vector v1, Vector v2)
        //{
        //    return Math.Round(v1.X, 2) == Math.Round(v2.X, 2) ^
        //           Math.Round(v1.Y, 2) == Math.Round(v2.Y, 2) ^
        //           Math.Round(v1.Z, 2) == Math.Round(v2.Z, 2); // 0.995 e 1.005 são considerados como 1

        //}
        ///// <summary>
        ///// Retorna um booleano afirmando se os módulos dos vetores são diferentes
        ///// </summary>
        ///// <param name="v1">Vetor A</param>
        ///// <param name="v2">Vetor B</param>
        ///// <returns>Bool</returns>
        //public static bool operator !=(Vector v1, Vector v2)
        //{
        //    return Math.Round(v1.X, 2) != Math.Round(v2.X, 2) ||
        //           Math.Round(v1.Y, 2) != Math.Round(v2.Y, 2) ||
        //           Math.Round(v1.Z, 2) != Math.Round(v2.Z, 2); // 0.995 e 1.005 são considerados como 1
        //}
        #endregion

        #region Methods
        /// <summary>
        /// ctor
        /// </summary>
        public Vector() : this(0f, 0f, 0f)
        {

        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="x">Valor no eixo X</param>
        /// <param name="y">Valor no eixo Y</param>
        /// <param name="z">Valor no eixo Z</param>
        public Vector(float x = 0, float y = 0, float z = 0)
        {
            Set(x, y, z);
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="x">Valor no eixo X</param>
        /// <param name="y">Valor no eixo Y</param>
        /// <param name="z">Valor no eixo Z</param>
        public Vector(double x = 0, double y = 0, double z = 0)
        {
            Set((float)x, (float)y, (float)z);
        }

        /// <summary>
        /// Define os valores dos eixos do vetor
        /// </summary>
        /// <param name="x">Valor no eixo X</param>
        /// <param name="y">Valor no eixo Y</param>
        /// <param name="z">Valor no eixo Z</param>
        public void Set(float x, float y, float z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Retorna o vetor direcionado a um ponto
        /// </summary>
        /// <param name="position">Ponto a se direcionar</param>
        /// <returns></returns>
        public Vector PointedAt(Vector position)
        {
            return this - position;
        }

        /// <summary>
        /// Retorna o módulo do resultado da soma entre dois vetores
        /// </summary>
        /// <param name="v">Segundo vetor</param>
        /// <returns>Float</returns>
        public float DistanceBetween(Vector v)
        {
            return (this - v).Magnitude;
        }

        /// <summary>
        /// Retorna o ângulo entre dois vetores, em radianos
        /// </summary>
        /// <param name="v">Segundo vetor</param>
        /// <returns>Ângulo em radianos</returns>
        public float AngleBetween(Vector v)
        {
            return MathExtension.DegreesToRadians((float)System.Windows.Vector.AngleBetween(
                new System.Windows.Vector(X, Y), 
                new System.Windows.Vector(v.X, v.Y)));
        }

        /// <summary>
        /// Torna o vetor unitário
        /// </summary>
        public void Normalize()
        {
            var normalized = Normalized;
            Set(normalized.X, normalized.Y, normalized.Z);
        }

        /// <summary>
        /// Aproxima os eixos do vetor aos eixos de um outro
        /// </summary>
        /// <param name="v">Vetor alvo</param>
        /// <param name="offset">Deslocamento máximo de eixo</param>
        public void Approximate(Vector v, float offset)
        {
            if (Math.Abs(X - v.X) <= offset)
                X = v.X;
            else if (Math.Abs(X - v.X) != 0)
            {
                if (X > v.X)
                    X -= offset;
                else if (X < v.X)
                    X += offset;
            }

            if (Math.Abs(Y - v.Y) <= offset)
                Y = v.Y;
            else if (Math.Abs(Y - v.Y) != 0)
            {
                if (Y > v.Y)
                    Y -= offset;
                else if (Y < v.Y)
                    Y += offset;
            }

            if (Math.Abs(Z - v.Z) <= offset)
                Z = v.Z;
            else if (Math.Abs(Z - v.Z) != 0)
            {
                if (Z > v.Z)
                    Z -= offset;
                else if (Z < v.Z)
                    Z += offset;
            }
        }
        #endregion
    }
}
