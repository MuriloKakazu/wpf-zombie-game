using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZombieGame.Physics.Enums;

namespace ZombieGame.Physics.Extensions
{
    public static class RectExtensions
    {
        /// <summary>
        /// Retorna um ponto do retângulo em forma de vetor
        /// </summary>
        /// <param name="r">Retângulo</param>
        /// <param name="pos">Ponto</param>
        /// <returns>Vetor</returns>
        public static Vector GetVector(this Rect r, RectPositions pos)
        {
            if (pos == RectPositions.BottomLeft)
                return new Vector((float)r.TopLeft.X, (float)(r.TopLeft.Y - r.Height));
            else if (pos == RectPositions.BottomRight)
                return new Vector((float)r.TopRight.X, (float)(r.TopRight.Y - r.Height));
            else if (pos == RectPositions.TopLeft)
                return new Vector((float)r.TopLeft.X, (float)r.TopLeft.Y);
            else if (pos == RectPositions.TopRight)
                return new Vector((float)r.TopRight.X, (float)r.TopRight.Y);
            else if (pos == RectPositions.Center)
                return new Vector((float)(r.BottomRight.X - r.Width / 2), (float)(r.BottomRight.Y - r.Height * 1.5));
            else if (pos == RectPositions.CenterLeft)
                return new Vector((float)(r.BottomLeft.X), (float)(r.BottomRight.Y - r.Height * 1.5));
            else if (pos == RectPositions.CenterRight)
                return new Vector((float)(r.BottomRight.X), (float)(r.BottomRight.Y - r.Height * 1.5));
            else if (pos == RectPositions.CenterTop)
                return new Vector((float)(r.BottomRight.X - r.Width / 2), (float)(r.TopLeft.Y));
            else if (pos == RectPositions.CenterBottom)
                return new Vector((float)(r.BottomRight.X - r.Width / 2), (float)(r.TopRight.Y - r.Height));

            return null;
        }

        /// <summary>
        /// Retorna o retângulo relativo à janela do programa
        /// </summary>
        /// <param name="r">Retângulo</param>
        /// <returns>Retângulo</returns>
        public static Rect RelativeToWindow(this Rect r)
        {
            return new Rect(r.X, -r.Y, r.Width, r.Height);
        }
    }
}
