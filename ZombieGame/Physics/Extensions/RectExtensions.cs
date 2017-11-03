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
                return new Vector((float)r.BottomLeft.X, (float)r.BottomLeft.Y);
            else if (pos == RectPositions.BottomRight)
                return new Vector((float)r.BottomRight.X, (float)r.BottomRight.Y);
            else if (pos == RectPositions.TopLeft)
                return new Vector((float)r.TopLeft.X, (float)r.TopLeft.Y);
            else if (pos == RectPositions.TopRight)
                return new Vector((float)r.TopRight.X, (float)r.TopRight.Y);
            else if (pos == RectPositions.Center)
                return new Vector((float)(r.BottomRight.X - r.Width / 2), (float)(r.BottomRight.Y - r.Height / 2));
            else if (pos == RectPositions.CenterLeft)
                return new Vector(GetVector(r, RectPositions.Center).X - (float)(r.Width / 2), GetVector(r, RectPositions.Center).Y);
            else if (pos == RectPositions.CenterRight)
                return new Vector(GetVector(r, RectPositions.Center).X + (float)(r.Width /2 ), GetVector(r, RectPositions.Center).Y);
            else if (pos == RectPositions.CenterTop)
                return new Vector(GetVector(r, RectPositions.Center).X, GetVector(r, RectPositions.Center).Y - (float)(r.Height / 2));
            else if (pos == RectPositions.CenterBottom)
                return new Vector(GetVector(r, RectPositions.Center).X, GetVector(r, RectPositions.Center).Y + (float)(r.Height / 2));

            return null;
        }
    }
}
