using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame.Physics
{
    public class Bounds
    {
        public Vector Position { get; set; }
        public Vector Size { get; set; }
        public Vector TopLeft { get { return Position; } }
        public Vector TopRight { get { return new Vector(Size.X + Position.X, Position.Y); } }
        public Vector BottomLeft { get { return new Vector(Position.X, Size.Y + Position.Y); } }
        public Vector BottomRight { get { return Size + Position; } }
        public Vector Center { get { return BottomRight / 2f; } }

        public Bounds(Vector position, Vector size)
        {
            Position = position;
            Size = size;
        }

        public Bounds(float x, float y, float width, float height)
        {
            Position = new Vector(x, y);
            Size = new Vector(width, height);
        }

        public Bounds(float x, float y, float z, float width, float height, float depth)
        {
            Position = new Vector(x, y, z);
            Size = new Vector(width, height, depth);
        }

        public bool IsInside(Bounds b)
        {
            return TopLeft <= b.BottomRight && TopLeft >= b.BottomLeft ||
                   TopRight >= b.BottomRight && TopRight <= b.BottomLeft ||
                   BottomLeft <= b.TopRight && BottomLeft >= b.TopLeft ||
                   BottomRight >= b.TopLeft && BottomRight <= b.TopRight;
        }
    }
}
