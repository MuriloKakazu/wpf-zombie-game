using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZombieGame.Physics
{
    /// <summary>
    /// Obsoleto. Use o objeto Rect
    /// </summary>
    [Obsolete]
    public class Bounds
    {
        /// <summary>
        /// Retorna um retângulo baseado na posição e tamanho delimitados
        /// </summary>
        protected Rect Rectangle { get { return new Rect(Position.X, Position.Y, Size.X, Size.Y); } }
        /// <summary>
        /// Retorna a posição do ponto superior esquerdo do objeto
        /// </summary>
        public Vector Position { get; set; }
        /// <summary>
        /// Retorna o tamanho do objeto em pixels
        /// </summary>
        public Vector Size { get; set; }
        /// <summary>
        /// Retorna a posição do ponto superior esquerdo do objeto
        /// </summary>
        public Vector TopLeft { get { return Position; } }
        /// <summary>
        /// Retorna a posição do ponto superior direito do objeto
        /// </summary>
        public Vector TopRight { get { return new Vector(Size.X + Position.X, Position.Y); } }
        /// <summary>
        /// Retorna a posição do ponto inferior esquerdo do objeto
        /// </summary>
        public Vector BottomLeft { get { return new Vector(Position.X, Size.Y + Position.Y); } }
        /// <summary>
        /// Retorna a posição do ponto inferior direito do objeto
        /// </summary>
        public Vector BottomRight { get { return Size + Position; } }
        /// <summary>
        /// Retorna a posição do ponto central do objeto
        /// </summary>
        public Vector Center { get { return BottomRight / 2f; } }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="position">Posição do ponto superior esquerdo do objeto</param>
        /// <param name="size">Tamanho do objeto em pixels</param>
        public Bounds(Vector position, Vector size)
        {
            Position = position;
            Size = size;
        }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="x">Posição do ponto superior esquerdo do objeto no eixo X</param>
        /// <param name="y">Posição do ponto superior esquerdo do objeto no eixo Y</param>
        /// <param name="width">Largura do objeto em pixels</param>
        /// <param name="height">Altura do objeto em pixels</param>
        public Bounds(float x, float y, float width, float height)
        {
            Position = new Vector(x, y);
            Size = new Vector(width, height);
        }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="x">Posição do ponto superior esquerdo frontal do objeto no eixo X</param>
        /// <param name="y">Posição do ponto superior esquerdo frontal do objeto no eixo Y</param>
        /// <param name="z">Posição do ponto superior esquerdo frontal do objeto no eixo Z</param>
        /// <param name="width">Largura do objeto</param>
        /// <param name="height">Altura do objeto</param>
        /// <param name="depth">Profundidade do objeto</param>
        public Bounds(float x, float y, float z, float width, float height, float depth)
        {
            Position = new Vector(x, y, z);
            Size = new Vector(width, height, depth);
        }

        /// <summary>
        /// Retorna um booleano afirmando se o retângulo do objeto intersecciona com outro
        /// </summary>
        /// <param name="b">Retângulo do outro objeto</param>
        /// <returns>Bool</returns>
        public bool IntersectsWith(Bounds b)
        {
            return Rectangle.IntersectsWith(b.Rectangle);
        }
    }
}
